# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore first so dependency downloads are cached between builds
COPY Draveling.API/Draveling.API/Draveling.API.csproj Draveling.API/Draveling.API/
RUN dotnet restore Draveling.API/Draveling.API/Draveling.API.csproj

COPY Draveling.API/Draveling.API/ Draveling.API/Draveling.API/
RUN dotnet publish Draveling.API/Draveling.API/Draveling.API.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Cloud Run sends traffic to port 8080 and terminates HTTPS at its proxy.
# Forwarded headers let the app see the original scheme and client address.
ENV ASPNETCORE_HTTP_PORTS=8080 \
    ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
EXPOSE 8080

USER $APP_UID
ENTRYPOINT ["dotnet", "Draveling.API.dll"]
