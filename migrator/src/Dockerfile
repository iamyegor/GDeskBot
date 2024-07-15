FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY MigrationRetriever/MigrationRetriever.csproj MigrationRetriever
COPY MigrationApplier/MigrationApplier.csproj MigrationApplier
COPY Common/Common.csproj Common
RUN dotnet restore MigrationRetriever/MigrationRetriever.csproj
RUN dotnet restore MigrationApplier/MigrationApplier.csproj

COPY . .

RUN dotnet publish -c Release -o /app/publish
RUN cp -r MigrationApplier /app/publish/MigrationApplier

# Production image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app

COPY --from=build /app/publish .

ENV DOTNET_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "MigrationRetriever.dll"]