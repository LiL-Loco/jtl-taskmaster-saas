# Architektur

## Systemübersicht
JTL TaskMaster besteht aus mehreren Kernkomponenten:

1. **Backend API (ASP.NET Core)**
   - RESTful API
   - SignalR für Echtzeit-Kommunikation
   - JWT Authentication

2. **Frontend (React)**
   - Single Page Application
   - Material-UI Components
   - Redux für State Management

3. **Worker Service**
   - Job Ausführung
   - Task Scheduling
   - Status Updates

4. **Agent**
   - JTL-Wawi Integration
   - Lokale Ausführung
   - Sichere Kommunikation

5. **Datenbank (PostgreSQL)**
   - Multi-Tenant Architektur
   - Job & Task Speicherung
   - Audit Logging

## Kommunikationsfluss
[Hier Diagramm einfügen]

## Technologie-Stack
- Backend: .NET 8, ASP.NET Core
- Frontend: React, TypeScript
- Datenbank: PostgreSQL
- Cache: Redis (optional)
- Message Queue: RabbitMQ (optional)
