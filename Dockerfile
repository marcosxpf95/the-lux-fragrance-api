# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar o csproj e restaurar as dependências
COPY the-lux-fragrance-api/the-lux-fragrance-api.csproj ./the-lux-fragrance-api/
WORKDIR /app/the-lux-fragrance-api
RUN dotnet restore

# Copiar o restante dos arquivos e compilar
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/the-lux-fragrance-api/out .
ENTRYPOINT ["dotnet", "the-lux-fragrance-api.dll"]
