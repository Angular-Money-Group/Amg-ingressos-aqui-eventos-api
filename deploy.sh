#!/bin/bash
echo "Logging in to Docker Hub..."
echo ${{ secrets.DOCKERHUB_TOKEN }} | docker login -u ${{ secrets.DOCKERHUB_USERNAME }} --password-stdin

echo "Building Docker image..."
docker build -t eventos-tickets .

echo "Pushing Docker image to Docker Hub..."
docker push eventos-tickets

echo "Deploying Docker container to Hostinger VPS..."
ssh root@181.215.134.104 "cd Amg-ingressos-aqui-eventos-api && docker-compose pull && docker-compose up -d"
