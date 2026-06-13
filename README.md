# EcoWaste API

REST API para gestao de residuos reciclaveis e monitoramento ambiental com foco em regras ESG.

## Stack

- .NET 8 (ASP.NET Core Web API)
- Entity Framework Core + Pomelo (MySQL)
- JWT Bearer Authentication
- FluentValidation
- Swagger/OpenAPI
- xUnit + Moq
- Docker

## Arquitetura

O projeto segue separacao por camadas em pastas:

- `Controllers`
- `Models`
- `ViewModels`
- `Services`
- `Repositories`
- `Data`
- `Configurations`
- `Middlewares`
- `Migrations`

## Requisitos

- SDK .NET instalado (o projeto usa `global.json`)
- MySQL 8
- Docker e Docker Compose (opcional, para subir MySQL local)

## Configuracao

As configuracoes principais estao em:

- `EcoWasteApi/appsettings.json`
- `EcoWasteApi/appsettings.Development.json`

Ajuste principalmente:

- `ConnectionStrings:DefaultConnection`
- `Jwt:Key` (use uma chave forte)

## Subir banco MySQL com Docker

```bash
docker compose -f compose.yaml up -d mysql
```

## Migrations

Gerar migration:

```bash
DOTNET_ROLL_FORWARD=Major ~/.dotnet/tools/dotnet-ef migrations add InitialCreate --project EcoWasteApi/EcoWasteApi.csproj --startup-project EcoWasteApi/EcoWasteApi.csproj --output-dir Migrations
```

Aplicar migrations:

```bash
DOTNET_ROLL_FORWARD=Major ~/.dotnet/tools/dotnet-ef database update --project EcoWasteApi/EcoWasteApi.csproj --startup-project EcoWasteApi/EcoWasteApi.csproj
```

> A aplicacao tambem executa `Database.Migrate()` automaticamente no startup.

## Executar aplicacao

```bash
cd EcoWasteApi
DOTNET_ROLL_FORWARD=Major dotnet run
```

## Executar testes

```bash
DOTNET_ROLL_FORWARD=Major dotnet test EcoWasteApi.sln
```

## Insomnia

Foi gerada uma colecao importavel em:

- `insomnia-ecowaste.json`

A colecao inclui:

- todos os endpoints da API
- variaveis `base_url` e `jwt_token`
- exemplos de payload para `POST` e `PUT`
- organizacao por pastas (Authentication, Recycling Centers, Waste Records)

## Endpoints principais

### Authentication

- `POST /api/auth/register`
- `POST /api/auth/login`

### Recycling Centers

- `POST /api/recycling-centers`
- `GET /api/recycling-centers`
- `GET /api/recycling-centers/{id}`
- `PUT /api/recycling-centers/{id}`
- `DELETE /api/recycling-centers/{id}` (Admin)

### Waste Records

- `POST /api/waste-records`
- `GET /api/waste-records`
- `GET /api/waste-records/{id}`
- `PUT /api/waste-records/{id}`
- `DELETE /api/waste-records/{id}` (Admin)
- `GET /api/waste-records/alerts`
- `GET /api/waste-records/report`


