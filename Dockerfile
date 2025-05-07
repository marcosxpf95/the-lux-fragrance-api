# Etapa de build usando o SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar apenas o .csproj e restaurar dependências
COPY the-lux-fragrance-api/the-lux-fragrance-api/the-lux-fragrance-api.csproj ./the-lux-fragrance-api/
WORKDIR /app/the-lux-fragrance-api
RUN dotnet restore

# Copiar o restante dos arquivos e compilar
WORKDIR /app
COPY . ./
RUN dotnet publish the-lux-fragrance-api/the-lux-fragrance-api/the-lux-fragrance-api.csproj -c Release -o /app/out

# Etapa de runtime com a imagem do ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Define a porta dinâmica do Heroku e inicia a aplicação
ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE $PORT
CMD ["dotnet", "the-lux-fragrance-api.dll"]
