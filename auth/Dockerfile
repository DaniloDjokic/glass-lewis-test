FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-stage

WORKDIR /app

COPY . .
RUN dotnet restore

RUN dotnet publish AuthServer.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

COPY --from=build-stage /app/publish .

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

EXPOSE 8080

ENTRYPOINT ["dotnet", "AuthServer.dll"]
