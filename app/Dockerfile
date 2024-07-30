# Build Image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Api/Api.csproj Api/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY DialogProcessing/DialogProcessing.csproj DialogProcessing/

RUN dotnet restore Api/Api.csproj

COPY . .

WORKDIR /src/Api
RUN dotnet publish -c Release -o /app/publish

COPY Infrastructure/Data/Migrations /app/publish/Migrations

# Production image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "Api.dll"]