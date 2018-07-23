FROM microsoft/aspnetcore:2.0-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-sac2016 AS build
WORKDIR /src
COPY Services/AccuMarkOrders/AccuMarkOrders.csproj Services/AccuMarkOrders/
RUN dotnet restore Services/AccuMarkOrders/AccuMarkOrders.csproj
COPY . .
WORKDIR /src/Services/AccuMarkOrders
RUN dotnet build AccuMarkOrders.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AccuMarkOrders.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AccuMarkOrders.dll"]
