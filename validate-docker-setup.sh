#!/bin/bash

echo "================================================"
echo "MiniGED Docker Setup Validation"
echo "================================================"
echo ""

# Check if Docker is installed
echo "✓ Checking Docker installation..."
if ! command -v docker &> /dev/null; then
    echo "✗ Docker is not installed. Please install Docker first."
    exit 1
fi
echo "  Docker version: $(docker --version)"
echo ""

# Check if Docker Compose is available
echo "✓ Checking Docker Compose..."
if docker compose version &> /dev/null; then
    echo "  Docker Compose version: $(docker compose version)"
elif command -v docker-compose &> /dev/null; then
    echo "  Docker Compose version: $(docker-compose --version)"
else
    echo "✗ Docker Compose is not available"
    exit 1
fi
echo ""

# Check if docker-compose.yml exists
echo "✓ Checking docker-compose.yml..."
if [ ! -f "docker-compose.yml" ]; then
    echo "✗ docker-compose.yml not found in current directory"
    exit 1
fi
echo "  docker-compose.yml found"
echo ""

# Validate docker-compose.yml
echo "✓ Validating docker-compose.yml..."
if docker compose config > /dev/null 2>&1 || docker-compose config > /dev/null 2>&1; then
    echo "  docker-compose.yml is valid"
else
    echo "✗ docker-compose.yml has errors"
    exit 1
fi
echo ""

# Check Dockerfile existence
echo "✓ Checking Dockerfiles..."
if [ -f "MiniGED-Frontend/Dockerfile" ]; then
    echo "  Frontend Dockerfile found"
else
    echo "✗ Frontend Dockerfile not found"
fi

if [ -f "MiniGED.Backend/Dockerfile" ]; then
    echo "  Backend Dockerfile found"
else
    echo "✗ Backend Dockerfile not found"
fi
echo ""

echo "================================================"
echo "All checks passed! You can now run:"
echo "  docker compose up -d"
echo ""
echo "Or use Docker Buildx for cloud builds:"
echo "  docker buildx create --name miniged-builder --driver docker-container --bootstrap --use"
echo "  docker compose up -d --build"
echo "================================================"
