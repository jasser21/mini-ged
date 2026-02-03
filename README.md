# MiniGED - Document Management System

MiniGED is a document management system with a Vue.js frontend and .NET backend.

## Running with Docker

The entire application stack can be run using Docker Compose.

### Prerequisites

- Docker
- Docker Compose

### Quick Start

1. Clone the repository
2. Navigate to the root directory
3. Run the following command:

```bash
docker-compose up -d
```

This will start all services:
- **Frontend**: Available at http://localhost:80
- **Backend API**: Available at http://localhost:8080
- **Swagger UI**: Available at http://localhost:8080/swagger (Development mode)
- **Meilisearch**: Available at http://localhost:7700
- **SQL Server**: Available at localhost:1433

### Services

#### Frontend (Vue.js + Vite)
- Port: 80
- Built with Nginx for production
- Uses environment variable `VITE_API_BASE_URL` to connect to backend

#### Backend (.NET 9.0)
- Ports: 8080 (HTTP), 8081 (HTTPS)
- Uses SQL Server for database
- Uses Hangfire for background jobs (SQLite)
- Uses Meilisearch for document search

#### SQL Server
- Port: 1433
- Username: sa
- Password: Test123!@#Strong
- Database: MiniGed

#### Meilisearch
- Port: 7700
- Master Key: AA9shxrd4zOvVJ31NKOoEhC_p9h01XqcyJfmVth7H50

### Stopping the Services

```bash
docker-compose down
```

### Rebuilding After Changes

```bash
docker-compose up -d --build
```

### Viewing Logs

```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f backend
docker-compose logs -f frontend
```

### Development

For local development without Docker, see the individual README files in:
- `MiniGED-Frontend/` - Frontend development instructions
- `MiniGED.Backend/` - Backend development instructions
