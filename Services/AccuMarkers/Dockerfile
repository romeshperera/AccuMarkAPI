FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY Services/AccuMarkers/AccuMarkers.csproj Services/AccuMarkers/
COPY Core/PIWapper/PIWapper.csproj Core/SampleWapper/
RUN dotnet restore Services/AccuMarkers/AccuMarkers.csproj
COPY . .
WORKDIR /src/Services/AccuMarkers
RUN dotnet build AccuMarkers.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AccuMarkers.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AccuMarkers.dll"]
