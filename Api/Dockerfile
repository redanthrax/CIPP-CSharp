# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["Api/CIPP.Api.csproj", "Api/"]
COPY ["Shared/CIPP.Shared.csproj", "Shared/"]

# Restore dependencies
RUN dotnet restore "Api/CIPP.Api.csproj"

# Copy source code
COPY . .

# Build the application
WORKDIR "/src/Api"
RUN dotnet build "CIPP.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "CIPP.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create non-root user
RUN adduser --disabled-password --home /app --gecos '' cippuser && chown -R cippuser /app
USER cippuser

# Copy published application
COPY --from=publish /app/publish .

# Expose port
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=30s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "CIPP.Api.dll"]