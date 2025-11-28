# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["Frontend/CIPP.Frontend.csproj", "Frontend/"]
COPY ["Shared/CIPP.Shared.csproj", "Shared/"]

# Restore dependencies
RUN dotnet restore "Frontend/CIPP.Frontend.csproj"

# Copy source code
COPY . .

# Build the application
WORKDIR "/src/Frontend"
RUN dotnet build "CIPP.Frontend.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "CIPP.Frontend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage - use nginx to serve static files
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Install wget for health check
RUN apk add --no-cache wget

# Copy published Blazor WASM files
COPY --from=publish /app/publish/wwwroot .

# Create nginx config for SPA routing with HTTPS support
RUN echo 'server { \
    listen 8080; \
    listen 8443 ssl; \
    server_name _; \
    root /usr/share/nginx/html; \
    index index.html; \
    \
    ssl_certificate /https/aspnetapp.crt; \
    ssl_certificate_key /https/aspnetapp.key; \
    ssl_protocols TLSv1.2 TLSv1.3; \
    ssl_prefer_server_ciphers on; \
    \
    location / { \
        try_files $uri $uri/ /index.html =404; \
    } \
}' > /etc/nginx/conf.d/default.conf

# Expose ports
EXPOSE 8080 8443

# Health check
HEALTHCHECK --interval=30s --timeout=30s --start-period=5s --retries=3 \
  CMD wget --quiet --tries=1 --spider http://localhost:8080 || exit 1

# Run nginx
CMD ["nginx", "-g", "daemon off;"]
