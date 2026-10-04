FROM mcr.microsoft.com/dotnet/runtime:10.0-alpine AS base
RUN mkdir -p /app && chown $APP_UID /app
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src
COPY . .
RUN dotnet restore Worker/Worker.csproj
RUN dotnet publish Worker/Worker.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Worker.dll"]
