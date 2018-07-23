FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY Services/AccuMarkStorageArea/AccuMarkStorageArea.csproj Services/AccuMarkStorageArea/
RUN dotnet restore Services/AccuMarkStorageArea/AccuMarkStorageArea.csproj
COPY . .
WORKDIR /src/Services/AccuMarkStorageArea
RUN dotnet build AccuMarkStorageArea.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AccuMarkStorageArea.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AccuMarkStorageArea.dll"]
