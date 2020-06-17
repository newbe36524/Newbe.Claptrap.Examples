#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
# silo
EXPOSE 11111
# gateway
EXPOSE 30000
# dashboard
EXPOSE 9000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY ["Newbe.Claptrap.Auth.BackendServer/Newbe.Claptrap.Auth.BackendServer.csproj", "Newbe.Claptrap.Auth.BackendServer/"]
COPY ["Newbe.Claptrap.Auth.Grains/Newbe.Claptrap.Auth.Grains.csproj", "Newbe.Claptrap.Auth.Grains/"]
COPY ["Newbe.Claptrap.Auth.IGrains/Newbe.Claptrap.Auth.IGrains.csproj", "Newbe.Claptrap.Auth.IGrains/"]
COPY ["Newbe.Claptrap.Auth.Models/Newbe.Claptrap.Auth.Models.csproj", "Newbe.Claptrap.Auth.Models/"]
COPY ["Newbe.Claptrap.Auth.Repository/Newbe.Claptrap.Auth.Repository.csproj", "Newbe.Claptrap.Auth.Repository/"]
COPY .nuget.config ./nuget.config
RUN dotnet nuget disable source nuget.org && dotnet restore "Newbe.Claptrap.Auth.BackendServer/Newbe.Claptrap.Auth.BackendServer.csproj"
COPY . .
WORKDIR "/src/Newbe.Claptrap.Auth.BackendServer"
RUN dotnet build "Newbe.Claptrap.Auth.BackendServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Newbe.Claptrap.Auth.BackendServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Newbe.Claptrap.Auth.BackendServer.dll"]
