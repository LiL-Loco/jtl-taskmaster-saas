# Deployment Guide

## Voraussetzungen
- .NET 8 SDK
- Node.js 18+
- Docker
- PostgreSQL 15+
- Nginx

## Installation

### 1. Backend API
cd src/api
dotnet publish -c Release

### 2. Worker Service
cd src/worker
dotnet publish -c Release

### 3. Frontend
cd src/frontend
npm install
npm run build

### 4. Agent
cd src/agent
dotnet publish -c Release

## Konfiguration

### Nginx Konfiguration
server {
    listen 80;
    server_name your-domain.com;
    
    location / {
        root /var/www/frontend;
        try_files $uri $uri/ /index.html;
    }
    
    location /api {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
