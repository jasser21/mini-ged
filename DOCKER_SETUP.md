# Dockerization Summary

This document summarizes the changes made to dockerize the MiniGED application.

## Changes Made

### 1. Frontend Dockerization
- **Created `MiniGED-Frontend/Dockerfile`**: Multi-stage Dockerfile using Node.js Alpine for building and Nginx Alpine for serving
- **Created `MiniGED-Frontend/nginx.conf`**: Nginx configuration with gzip compression, SPA routing support, and caching
- **Created `MiniGED-Frontend/.dockerignore`**: Excludes unnecessary files from Docker build context
- **Created `MiniGED-Frontend/.env`**: Development environment configuration for API base URL
- **Created `MiniGED-Frontend/.env.production`**: Production environment configuration for API base URL

### 2. Frontend Code Updates
- Updated API service files to use environment variables instead of hardcoded URLs:
  - `src/services/api.js`: Uses `VITE_API_BASE_URL` environment variable
  - `src/services/auth.js`: Uses `VITE_API_BASE_URL` environment variable  
  - `src/services/documentService.js`: Removed hardcoded baseURL overrides
  - `src/components/FileView.vue`: Uses `VITE_API_BASE_URL` for PDF URLs

### 3. Docker Compose Configuration
- **Created `docker-compose.yml` at root**: Comprehensive orchestration file with 4 services:
  - **sqlserver**: SQL Server 2022 Express for backend database
  - **meilisearch**: Search engine service
  - **backend**: .NET 9.0 API service
  - **frontend**: Vue.js/Vite application served by Nginx
- Configured proper networking, health checks, and service dependencies
- Added volume mounts for data persistence
- **Removed `MiniGED.Backend/docker-compose.yml`**: Old file only contained Meilisearch

### 4. Documentation
- **Created `README.md` at root**: Comprehensive documentation including:
  - Quick start guide
  - Docker Buildx cloud build instructions (addressing the new requirement)
  - Service descriptions and ports
  - Troubleshooting section for network issues
  - Development instructions
- **Created `.env.example`**: Template for environment variables
- **Created `validate-docker-setup.sh`**: Validation script to check Docker setup

## Architecture

The dockerized solution consists of 4 containers:

```
┌─────────────────┐
│   Frontend      │  Port 80
│   (Nginx)       │
└────────┬────────┘
         │
         │ HTTP API calls
         ↓
┌─────────────────┐
│    Backend      │  Ports 8080, 8081
│   (.NET API)    │
└────┬────┬───────┘
     │    │
     │    └──────────────┐
     ↓                   ↓
┌─────────────┐   ┌─────────────┐
│ SQL Server  │   │ Meilisearch │
│  (Database) │   │   (Search)  │
└─────────────┘   └─────────────┘
  Port 1433          Port 7700
```

## Docker Buildx Support

The new requirement to support cloud builds is addressed through Docker Buildx:

```bash
# Create builder
docker buildx create --name miniged-builder --driver docker-container --bootstrap --use

# Build images
docker buildx build --platform linux/amd64 -t miniged-frontend:latest ./MiniGED-Frontend --load
docker buildx build --platform linux/amd64 -t miniged-backend:latest ./MiniGED.Backend --load
```

Benefits:
- More robust network handling
- Better caching
- Multi-platform support
- Cloud-based builds possible with additional configuration

## Environment Variables

### Frontend
- `VITE_API_BASE_URL`: Backend API base URL (default: http://localhost:8080/api)

### Backend  
- `ASPNETCORE_ENVIRONMENT`: Environment mode (Development/Production)
- `ConnectionStrings__DefaultConnection`: SQL Server connection string
- `ConnectionStrings__HangfireConnection`: Hangfire database path
- `Meilisearch__Url`: Meilisearch service URL
- `Meilisearch__MasterKey`: Meilisearch master key

## Usage

### Start all services
```bash
docker compose up -d
```

### Stop all services
```bash
docker compose down
```

### View logs
```bash
docker compose logs -f [service-name]
```

### Rebuild after changes
```bash
docker compose up -d --build
```

## Testing

Run the validation script to verify Docker setup:
```bash
./validate-docker-setup.sh
```

## Notes

- The backend Dockerfile already existed and works correctly
- SQL Server is configured with Express edition for development
- All services use health checks for proper startup ordering
- Frontend is optimized for production with multi-stage build
- Network timeout issues during build can be mitigated using Buildx
