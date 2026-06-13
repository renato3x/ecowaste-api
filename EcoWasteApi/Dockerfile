# TODO - Consertar o Dockerfile para compilar corretamente
# TODO - Fazer com que a build aponte para o banco de dados do docker-compose para adicionar ao compose.yml
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EcoWasteApi/EcoWasteApi.csproj", "EcoWasteApi/"]
RUN dotnet restore "EcoWasteApi/EcoWasteApi.csproj"
COPY . .
WORKDIR "/src/EcoWasteApi"
RUN dotnet build "./EcoWasteApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EcoWasteApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EcoWasteApi.dll"]
