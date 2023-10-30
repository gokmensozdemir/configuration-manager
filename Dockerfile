#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ConfigurationManager.API/ConfigurationManager.API.csproj", "ConfigurationManager.API/"]
RUN dotnet restore "ConfigurationManager.API/ConfigurationManager.API.csproj"
COPY . .
WORKDIR "/src/ConfigurationManager.API"
RUN dotnet build "ConfigurationManager.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConfigurationManager.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConfigurationManager.API.dll"]