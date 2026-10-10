#!/usr/bin/env bash
# Compartilha o mesmo deploy e as mesmas verificações entre os dois ambientes.
set -euo pipefail

environment=${1:?Informe staging ou production}
export API_PORT=${2:?Informe a porta da API}
case "$environment" in
  staging) prefix=STAGING ;;
  production) prefix=PRODUCTION ;;
  *) echo "Ambiente inválido: $environment" >&2; exit 1 ;;
esac

for setting in MYSQL_ROOT_PASSWORD MYSQL_PASSWORD JWT_KEY; do
  variable="${prefix}_${setting}"
  export "$setting=${!variable:?Credencial temporária não gerada}"
done
export MYSQL_DATABASE="ecowaste_$environment"
export MYSQL_USER=ecowaste
export ASPNETCORE_ENVIRONMENT=Production
: "${API_IMAGE:?Imagem construída não informada}"

compose=(docker compose -p "$environment")
temporary=$(mktemp -d)
trap 'rm -rf "$temporary"' EXIT

"${compose[@]}" up -d --no-build --pull never --wait --wait-timeout 180
base_url="http://127.0.0.1:$API_PORT"
ready=false
for attempt in {1..30}; do
  code=$(curl --silent --max-time 5 --output /dev/null --write-out '%{http_code}' \
    "$base_url/api/recycling-centers" || true)
  if [[ "$code" == 401 ]]; then ready=true; break; fi
  sleep 2
done
if [[ "$ready" != true ]]; then
  echo "API de $environment não ficou pronta dentro do limite." >&2
  exit 1
fi

password=$(openssl rand -hex 32)
if [[ "${GITHUB_ACTIONS:-false}" == true ]]; then
  echo "::add-mask::$password"
fi
code=$(jq -n --arg password "$password" --arg email "ci-$environment@example.com" \
  '{name:"Verificação CI/CD",email:$email,password:$password}' | \
  curl --silent --show-error --max-time 10 --output "$temporary/register.json" \
    --write-out '%{http_code}' --header 'Content-Type: application/json' \
    --data-binary @- "$base_url/api/auth/register")
test "$code" = 201
token=$(jq -n --arg password "$password" --arg email "ci-$environment@example.com" \
  '{email:$email,password:$password}' | \
  curl --fail --silent --show-error --max-time 10 \
    --header 'Content-Type: application/json' --data-binary @- "$base_url/api/auth/login" | \
  jq -er '.access_token | select(type == "string" and length > 0)')
if [[ "${GITHUB_ACTIONS:-false}" == true ]]; then
  echo "::add-mask::$token"
  echo "${prefix}_SMOKE_PASSWORD=$password" >> "$GITHUB_ENV"
fi

for resource in recycling-centers waste-records; do
  curl --fail --silent --show-error --max-time 10 \
    --header "Authorization: Bearer $token" "$base_url/api/$resource" | \
    jq -e '.total_records >= 3 and (.items | length >= 3)' >/dev/null
  echo "$environment: GET /api/$resource autenticado retornou HTTP 200 e dados das migrations."
done

expected=$(for migration in EcoWasteApi/Migrations/[0-9]*.cs; do
  [[ "$migration" == *.Designer.cs ]] && continue
  basename "$migration" .cs
done | sort)
actual=$("${compose[@]}" exec -T mysql sh -c \
  'MYSQL_PWD="$MYSQL_PASSWORD" mysql --protocol=TCP -h 127.0.0.1 -u "$MYSQL_USER" "$MYSQL_DATABASE" -N -e "SELECT MigrationId FROM __EFMigrationsHistory ORDER BY MigrationId"')
test "$actual" = "$expected"
mysql=$("${compose[@]}" ps -q mysql)
api=$("${compose[@]}" ps -q api)
test "$(docker inspect "$mysql" --format '{{.State.Health.Status}}')" = healthy
test "$(docker inspect "$api" --format '{{.State.Status}}')" = running
test "$(docker inspect "$api" --format '{{.RestartCount}}')" = 0
test "$(docker inspect "$api" --format '{{.Image}}')" = \
  "$(docker image inspect "$API_IMAGE" --format '{{.Id}}')"
"${compose[@]}" ps
printf '%s: migrations confirmadas:\n%s\n' "$environment" "$actual"
echo "$environment: cadastro HTTP 201, login HTTP 200, autenticação HTTP 401 sem token, MySQL healthy e API running: OK."
