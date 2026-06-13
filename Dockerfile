FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["EcoWasteApi/EcoWasteApi.csproj", "EcoWasteApi/"]
RUN dotnet restore "EcoWasteApi/EcoWasteApi.csproj"

COPY . .
WORKDIR /src/EcoWasteApi
RUN dotnet publish "EcoWasteApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "EcoWasteApi.dll"]

