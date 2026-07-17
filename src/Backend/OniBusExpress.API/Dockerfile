FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Backend/OniBusExpress.API/OniBusExpress.API.csproj src/Backend/OniBusExpress.API/
COPY src/Backend/OniBusExpress.Application/OniBusExpress.Application.csproj src/Backend/OniBusExpress.Application/
COPY src/Backend/OniBusExpress.Domain/OniBusExpress.Domain.csproj src/Backend/OniBusExpress.Domain/
COPY src/Backend/OniBusExpress.Infrastructure/OniBusExpress.Infrastructure.csproj src/Backend/OniBusExpress.Infrastructure/
COPY src/Shared/OniBusExpress.Communication/OniBusExpress.Communication.csproj src/Shared/OniBusExpress.Communication/
COPY src/Shared/OniBusExpress.Exceptions/OniBusExpress.Exceptions.csproj src/Shared/OniBusExpress.Exceptions/

RUN dotnet restore src/Backend/OniBusExpress.API/OniBusExpress.API.csproj

COPY src/ src/

RUN dotnet publish src/Backend/OniBusExpress.API/OniBusExpress.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "OniBusExpress.API.dll"]
