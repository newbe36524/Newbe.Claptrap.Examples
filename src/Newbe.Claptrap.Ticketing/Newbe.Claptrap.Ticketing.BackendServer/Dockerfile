#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /src
COPY ["Newbe.Claptrap.Ticketing.BackendServer/Newbe.Claptrap.Ticketing.BackendServer.csproj", "Newbe.Claptrap.Ticketing.BackendServer/"]
COPY ["Newbe.Claptrap.Ticketing.Actors/Newbe.Claptrap.Ticketing.Actors.csproj", "Newbe.Claptrap.Ticketing.Actors/"]
COPY ["Newbe.Claptrap.Ticketing.Repository/Newbe.Claptrap.Ticketing.Repository.csproj", "Newbe.Claptrap.Ticketing.Repository/"]
COPY ["Newbe.Claptrap.Ticketing.IActor/Newbe.Claptrap.Ticketing.IActor.csproj", "Newbe.Claptrap.Ticketing.IActor/"]
COPY ["Newbe.Claptrap.Ticketing.Models/Newbe.Claptrap.Ticketing.Models.csproj", "Newbe.Claptrap.Ticketing.Models/"]
COPY .nuget.config nuget.config
RUN dotnet restore "Newbe.Claptrap.Ticketing.BackendServer/Newbe.Claptrap.Ticketing.BackendServer.csproj"
COPY . .
WORKDIR "/src/Newbe.Claptrap.Ticketing.BackendServer"
RUN dotnet build "Newbe.Claptrap.Ticketing.BackendServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Newbe.Claptrap.Ticketing.BackendServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Newbe.Claptrap.Ticketing.BackendServer.dll"]