#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Newbe.Claptrap.Shop/Newbe.Claptrap.Shop.csproj", "Newbe.Claptrap.Shop/"]
COPY ["Newbe.Claptrap.Shop.Grains/Newbe.Claptrap.Shop.Grains.csproj", "Newbe.Claptrap.Shop.Grains/"]
COPY ["Newbe.Claptrap.Shop.Repository/Newbe.Claptrap.Shop.Repository.csproj", "Newbe.Claptrap.Shop.Repository/"]
COPY ["Newbe.Claptrap.Shop.Models/Newbe.Claptrap.Shop.Models.csproj", "Newbe.Claptrap.Shop.Models/"]
COPY ["Newbe.Claptrap.Shop.IGrains/Newbe.Claptrap.Shop.IGrains.csproj", "Newbe.Claptrap.Shop.IGrains/"]
RUN dotnet restore "Newbe.Claptrap.Shop/Newbe.Claptrap.Shop.csproj"
COPY . .
WORKDIR "/src/Newbe.Claptrap.Shop"
RUN dotnet build "Newbe.Claptrap.Shop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Newbe.Claptrap.Shop.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Newbe.Claptrap.Shop.dll"]