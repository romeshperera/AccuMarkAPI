FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY Services/AccuBatch/AccuBatch.csproj Services/AccuBatch/
COPY Core/PIWapper/PIWapper.csproj Core/SampleWapper/
RUN dotnet restore Services/AccuBatch/AccuBatch.csproj
COPY . .
WORKDIR /src/Services/AccuBatch
RUN dotnet build AccuBatch.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AccuBatch.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AccuBatch.dll"]
