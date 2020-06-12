#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ARG instrumentation_key
ENV APPINSIGHTS_INSTRUMENTATIONKEY=$instrumentation_key

ARG app_insights_api_key
ENV APP_INSIGHTS_API_KEY=$app_insights_api_key

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY *.sln ./

COPY ["RYoshiga.Demo.Domain/*.csproj", "RYoshiga.Demo.Domain/"]
COPY ["RYoshiga.Demo.WebApi/*.csproj", "RYoshiga.Demo.WebApi/"]
RUN dotnet restore "RYoshiga.Demo.WebApi/RYoshiga.Demo.WebApi.csproj"
COPY . .

WORKDIR /src/RYoshiga.Demo.WebApi
RUN dotnet build "RYoshiga.Demo.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RYoshiga.Demo.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RYoshiga.Demo.WebApi.dll"]