FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY MigrationRetriever/MigrationRetriever.csproj MigrationRetriever/
COPY MigrationApplier/MigrationApplier.csproj MigrationApplier/
RUN dotnet restore MigrationRetriever/MigrationRetriever.csproj

COPY . .

RUN dotnet publish -c Release -o /app
RUN rm -f /app/MigrationApplier 
RUN cp -r MigrationApplier /app/MigrationApplier

WORKDIR /app

ENV DOTNET_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "MigrationRetriever.dll"]