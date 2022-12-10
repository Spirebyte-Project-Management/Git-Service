FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

ARG Nuget_CustomFeedUserName
ARG Nuget_CustomFeedPassword

WORKDIR /src
COPY ["src/Spirebyte.Services.Git.API/Spirebyte.Services.Git.API.csproj", "src/Spirebyte.Services.Git.API/"]
COPY ["src/Spirebyte.Services.Git.Infrastructure/Spirebyte.Services.Git.Infrastructure.csproj", "src/Spirebyte.Services.Git.Infrastructure/"]
COPY ["src/Spirebyte.Services.Git.Application/Spirebyte.Services.Git.Application.csproj", "src/Spirebyte.Services.Git.Application/"]
COPY ["src/Spirebyte.Services.Git.Core/Spirebyte.Services.Git.Core.csproj", "src/Spirebyte.Services.Git.Core/"]
RUN dotnet nuget add source --username ${Nuget_CustomFeedUserName} --password ${Nuget_CustomFeedPassword} --store-password-in-clear-text --name github "https://nuget.pkg.github.com/Spirebyte-Project-Management/index.json"
RUN dotnet restore "src/Spirebyte.Services.Git.API/Spirebyte.Services.Git.API.csproj"
COPY . .
WORKDIR "/src/src/Spirebyte.Services.Git.API"
RUN dotnet build "Spirebyte.Services.Git.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spirebyte.Services.Git.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT Docker
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED true
ENTRYPOINT ["dotnet", "Spirebyte.Services.Git.API.dll"]
