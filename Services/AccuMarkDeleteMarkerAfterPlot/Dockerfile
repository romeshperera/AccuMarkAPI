FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY Services/AccuMarkDeleteMarkerAfterPlot/AccuMarkDeleteMarkerAfterPlot.csproj Services/AccuMarkDeleteMarkerAfterPlot/
RUN dotnet restore Services/AccuMarkDeleteMarkerAfterPlot/AccuMarkDeleteMarkerAfterPlot.csproj
COPY . .
WORKDIR /src/Services/AccuMarkDeleteMarkerAfterPlot
RUN dotnet build AccuMarkDeleteMarkerAfterPlot.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AccuMarkDeleteMarkerAfterPlot.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AccuMarkDeleteMarkerAfterPlot.dll"]
