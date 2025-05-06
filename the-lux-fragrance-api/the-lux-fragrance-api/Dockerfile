# Etapa de build usando o SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar o csproj e restaurar as dependências
COPY *.csproj ./
RUN dotnet restore

# Copiar o restante dos arquivos e compilar
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa de runtime com a versão do ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Define a porta dinâmica do Heroku e inicia a aplicação
CMD ASPNETCORE_URLS="http://*:$PORT" dotnet the-lux-fragrance-api.dll