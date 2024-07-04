#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Product.Catalog.API/Product.Catalog.API.csproj", "Product.Catalog.API/"]
COPY ["Product.Catalog.Infra.IOC/Product.Catalog.Infra.IOC.csproj", "Product.Catalog.Infra.IOC/"]
COPY ["Product.Catalog.Infra.Data/Product.Catalog.Infra.Data.csproj", "Product.Catalog.Infra.Data/"]
COPY ["Products.Catalog.Domain/Products.Catalog.Domain.csproj", "Products.Catalog.Domain/"]
COPY ["Products.Catalog.Application/Products.Catalog.Application.csproj", "Products.Catalog.Application/"]
COPY ["Products.Catalog.Infra.Authentication/Products.Catalog.Infra.Authentication.csproj", "Products.Catalog.Infra.Authentication/"]
COPY ["Products.Catalog.Infra.Mapper/Products.Catalog.Infra.Mapper.csproj", "Products.Catalog.Infra.Mapper/"]
RUN dotnet restore "./Product.Catalog.API/Product.Catalog.API.csproj"
COPY . .
WORKDIR "/src/Product.Catalog.API"
RUN dotnet build "./Product.Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Product.Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Product.Catalog.API.dll"]