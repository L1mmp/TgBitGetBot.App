#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TgBitGetBot.App/TgBitGetBot.App.csproj", "TgBitGetBot.App/"]
COPY ["TgBitGetBot.Application/TgBitGetBot.Application.csproj", "TgBitGetBot.Application/"]
COPY ["TgBitGetBot.Domain/TgBitGetBot.Domain.csproj", "TgBitGetBot.Domain/"]
COPY ["TgBitGetBot.Infrastructure/TgBitGetBot.Infrastructure.csproj", "TgBitGetBot.Infrastructure/"]
COPY ["TgBitGetBot.DataAccess/TgBitGetBot.DataAccess.csproj", "TgBitGetBot.DataAccess/"]
RUN dotnet restore "TgBitGetBot.App/TgBitGetBot.App.csproj"
COPY . .
WORKDIR "/src/TgBitGetBot.App"
RUN dotnet build "TgBitGetBot.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TgBitGetBot.App.csproj" -c Release -o /app/publish /p:UseAppHost=false




FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TgBitGetBot.App.dll"]