#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
ARG nuget_config=.nuget.config
WORKDIR /src
COPY ["Newbe.Claptrap.Auth.WebApi/Newbe.Claptrap.Auth.WebApi.csproj", "Newbe.Claptrap.Auth.WebApi/"]
COPY $nuget_config ./nuget.config
RUN dotnet restore "Newbe.Claptrap.Auth.WebApi/Newbe.Claptrap.Auth.WebApi.csproj"
COPY . .
WORKDIR "/src/Newbe.Claptrap.Auth.WebApi"
RUN dotnet build "Newbe.Claptrap.Auth.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Newbe.Claptrap.Auth.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Newbe.Claptrap.Auth.WebApi.dll"]
