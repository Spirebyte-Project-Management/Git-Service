FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish src/Spirebyte.Services.Git.API -c release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0

RUN apt-get -y update
RUN apt-get -y install git

WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT dotnet Spirebyte.Services.Git.API.dll