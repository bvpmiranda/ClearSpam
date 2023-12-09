#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ClearSpamService/ClearSpamService.csproj", "ClearSpamService/"]
RUN dotnet restore "ClearSpamService/ClearSpamService.csproj"
COPY . .
WORKDIR "/src/ClearSpamService"
RUN dotnet build "ClearSpamService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClearSpamService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClearSpamService.dll"]