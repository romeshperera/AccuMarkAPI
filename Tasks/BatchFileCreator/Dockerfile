FROM microsoft/dotnet:2.0-runtime-nanoserver-sac2016 AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk-nanoserver-sac2016 AS build
WORKDIR /src
COPY Tasks/BatchFileCreator/BatchFileCreator.csproj Tasks/BatchFileCreator/
RUN dotnet restore Tasks/BatchFileCreator/BatchFileCreator.csproj
COPY . .
WORKDIR /src/Tasks/BatchFileCreator
RUN dotnet build BatchFileCreator.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish BatchFileCreator.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BatchFileCreator.dll"]
