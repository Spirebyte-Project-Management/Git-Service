#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Git-Service/src/Spirebyte.Services.Git.API/Spirebyte.Services.Git.API.csproj", "src/Git-Service/src/Spirebyte.Services.Git.API/"]
COPY ["src/Git-Service/src/Spirebyte.Services.Git.Infrastructure/Spirebyte.Services.Git.Infrastructure.csproj", "src/Git-Service/src/Spirebyte.Services.Git.Infrastructure/"]
COPY ["src/Git-Service/src/Spirebyte.Services.Git.Application/Spirebyte.Services.Git.Application.csproj", "src/Git-Service/src/Spirebyte.Services.Git.Application/"]
COPY ["src/Git-Service/src/Spirebyte.Services.Git.Core/Spirebyte.Services.Git.Core.csproj", "src/Git-Service/src/Spirebyte.Services.Git.Core/"]
RUN dotnet restore "src/Git-Service/src/Spirebyte.Services.Git.API/Spirebyte.Services.Git.API.csproj"
COPY . .
WORKDIR "/src/src/Git-Service/src/Spirebyte.Services.Git.API"
RUN dotnet build "Spirebyte.Services.Git.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spirebyte.Services.Git.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Spirebyte.Services.Git.API.dll"]
