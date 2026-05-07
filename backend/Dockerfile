FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CareerHub.slnx", "."]
COPY ["src/CareerHub.API/CareerHub.API.csproj",             "src/CareerHub.API/"]
COPY ["src/CareerHub.Application/CareerHub.Application.csproj", "src/CareerHub.Application/"]
COPY ["src/CareerHub.Domain/CareerHub.Domain.csproj",       "src/CareerHub.Domain/"]
COPY ["src/CareerHub.Infrastructure/CareerHub.Infrastructure.csproj", "src/CareerHub.Infrastructure/"]
COPY ["src/CareerHub.SharedKernel/CareerHub.SharedKernel.csproj", "src/CareerHub.SharedKernel/"]
COPY ["tests/CareerHub.Application.Tests/CareerHub.Application.Tests.csproj", "tests/CareerHub.Application.Tests/"]

RUN dotnet restore "src/CareerHub.API/CareerHub.API.csproj"

COPY . .

RUN dotnet publish src/CareerHub.API/CareerHub.API.csproj \
    -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "CareerHub.API.dll"]
