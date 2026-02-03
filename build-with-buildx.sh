#!/bin/bash

echo "================================================"
echo "MiniGED Docker Cloud Build Script"
echo "================================================"
echo ""
echo "This script uses Docker Buildx for robust cloud-based building."
echo ""

# Check if Docker Buildx is available
if ! docker buildx version &> /dev/null; then
    echo "✗ Docker Buildx is not available"
    exit 1
fi

echo "✓ Docker Buildx is available"
echo ""

# Create or use buildx builder
BUILDER_NAME="miniged-builder"
if docker buildx inspect "$BUILDER_NAME" &> /dev/null; then
    echo "✓ Builder '$BUILDER_NAME' already exists, using it..."
    docker buildx use "$BUILDER_NAME"
else
    echo "→ Creating new builder '$BUILDER_NAME'..."
    docker buildx create --name "$BUILDER_NAME" --driver docker-container --bootstrap --use
    if [ $? -eq 0 ]; then
        echo "✓ Builder created successfully"
    else
        echo "✗ Failed to create builder"
        exit 1
    fi
fi
echo ""

# Build frontend
echo "→ Building frontend image with Buildx..."
docker buildx build \
    --platform linux/amd64 \
    -t miniged-frontend:latest \
    ./MiniGED-Frontend \
    --load

if [ $? -eq 0 ]; then
    echo "✓ Frontend image built successfully"
else
    echo "✗ Frontend build failed"
    exit 1
fi
echo ""

# Build backend  
echo "→ Building backend image with Buildx..."
docker buildx build \
    --platform linux/amd64 \
    -t miniged-backend:latest \
    ./MiniGED.Backend \
    --load

if [ $? -eq 0 ]; then
    echo "✓ Backend image built successfully"
else
    echo "✗ Backend build failed"
    exit 1
fi
echo ""

echo "================================================"
echo "✓ All images built successfully!"
echo ""
echo "You can now start the services with:"
echo "  docker compose up -d"
echo ""
echo "To push images to a registry:"
echo "  docker tag miniged-frontend:latest your-registry/miniged-frontend:latest"
echo "  docker tag miniged-backend:latest your-registry/miniged-backend:latest"
echo "  docker push your-registry/miniged-frontend:latest"
echo "  docker push your-registry/miniged-backend:latest"
echo "================================================"
