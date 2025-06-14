FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-stage

WORKDIR /app

COPY *.sln .
COPY src/CompanyAPI/*.csproj src/CompanyAPI/
COPY src/Domain/*.csproj src/Domain/
COPY src/Application/*.csproj src/Application/
COPY src/Infrastructure/*.csproj src/Infrastructure/
COPY tests/*.csproj tests/

RUN dotnet restore

COPY src/ src/

RUN dotnet publish src/CompanyAPI -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

COPY --from=build-stage /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "CompanyAPI.dll"]
