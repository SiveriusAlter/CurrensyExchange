FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CurrencyExchange.API.csproj", "./"]
COPY ["../CurrencyExchange.Application/CurrencyExchange.Application.csproj", "../CurrencyExchange.Application/"]
COPY ["../CurrencyExchange.Core/CurrencyExchange.Core.csproj", "../CurrencyExchange.Core/"]
COPY ["../CurrencyExchange.Data/CurrencyExchange.Data.csproj", "../CurrencyExchange.Data/"]
RUN dotnet restore "CurrencyExchange.API.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "./CurrencyExchange.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CurrencyExchange.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurrencyExchange.API.dll"]
