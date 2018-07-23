FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY Services/AccuMarkMarkers/AccuMarkMarkers.csproj Services/AccuMarkMarkers/
RUN dotnet restore Services/AccuMarkMarkers/AccuMarkMarkers.csproj
COPY . .
WORKDIR /src/Services/AccuMarkMarkers
RUN dotnet build AccuMarkMarkers.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AccuMarkMarkers.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AccuMarkMarkers.dll"]
