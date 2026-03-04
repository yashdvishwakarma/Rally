# ============================================
# RallyAPI Dockerfile - Multi-stage build
# ============================================

# --- Stage 1: Build ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY . .

RUN dotnet restore src/RallyAPI.Host/RallyAPI.Host.csproj

RUN dotnet publish src/RallyAPI.Host/RallyAPI.Host.csproj \
    -c Debug \
    -o /app/publish \
    --no-restore

# --- Stage 2: Runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS runtime
WORKDIR /app

# Install curl (health checks) and openssl (key generation)
RUN apt-get update && apt-get install -y --no-install-recommends curl openssl && \
    rm -rf /var/lib/apt/lists/*

# Copy published app
COPY --from=build /app/publish .

# Generate RSA keys if they don't exist
RUN mkdir -p Keys && \
    openssl genpkey -algorithm RSA -out Keys/private.pem -pkeyopt rsa_keygen_bits:2048 && \
    openssl rsa -pubout -in Keys/private.pem -out Keys/public.pem

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "RallyAPI.Host.dll"]