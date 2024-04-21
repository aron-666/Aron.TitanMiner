# FROM debian:bookworm-20240408-slim

# USER root
# WORKDIR /app

# COPY ./bin/Release/net8.0/publish/linux-x64 .

# RUN mkdir -p /root/.titanedge/storage

# RUN apt-get update && apt-get install -y libicu-dev
# RUN apt-get install ca-certificates -y

# ENTRYPOINT ["./Aron.TintanMiner.Console"]


FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
USER root
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Aron.TintanMiner.Console.csproj .
RUN dotnet restore "Aron.TintanMiner.Console.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Aron.TintanMiner.Console.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Aron.TintanMiner.Console.csproj" -c $BUILD_CONFIGURATION -o /app/publish  /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN mkdir -p /root/.titanedge/storage

RUN apt-get update && apt-get install -y libicu-dev
RUN apt-get install ca-certificates -y
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Aron.TintanMiner.Console.dll"]