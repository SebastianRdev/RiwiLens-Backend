FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RiwiLens.Api/RiwiLens.Api.csproj", "RiwiLens.Api/"]
RUN dotnet restore "RiwiLens.Api/RiwiLens.Api.csproj"
COPY . .
WORKDIR "/src/RiwiLens.Api"
RUN dotnet build "./RiwiLens.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RiwiLens.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RiwiLens.Api.dll"]
