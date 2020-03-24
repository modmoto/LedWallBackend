FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
COPY ./LedWallBackend.sln ./

COPY ./LedWallBackend/LedWallBackend.csproj ./LedWallBackend/LedWallBackend.csproj
RUN dotnet restore ./LedWallBackend/LedWallBackend.csproj

COPY ./LedWallBackend ./LedWallBackend
RUN dotnet build ./LedWallBackend/LedWallBackend.csproj -c Release

RUN dotnet publish "./LedWallBackend/LedWallBackend.csproj" -c Release -o "../../app/out"

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT dotnet LedWallBackend.dll testEnv=eins testEnv=zwei mongoConnectionString=$MONGO_CONNECTION_STRING