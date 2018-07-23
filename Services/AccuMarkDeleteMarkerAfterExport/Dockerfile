FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY Services/AccuMarkDeleteMarkerAfterExport/AccuMarkDeleteMarkerAfterExport.csproj Services/AccuMarkDeleteMarkerAfterExport/
RUN dotnet restore Services/AccuMarkDeleteMarkerAfterExport/AccuMarkDeleteMarkerAfterExport.csproj
COPY . .
WORKDIR /src/Services/AccuMarkDeleteMarkerAfterExport
RUN dotnet build AccuMarkDeleteMarkerAfterExport.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AccuMarkDeleteMarkerAfterExport.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AccuMarkDeleteMarkerAfterExport.dll"]
