# API Dokumentation

## Authentication
Die API verwendet JWT (JSON Web Tokens) für die Authentifizierung.

### Endpoints

#### Auth
POST /api/auth/login
POST /api/auth/register
POST /api/auth/refresh-token

#### Jobs
GET    /api/jobs
POST   /api/jobs
GET    /api/jobs/{id}
PUT    /api/jobs/{id}
DELETE /api/jobs/{id}
POST   /api/jobs/{id}/start

#### Tasks
GET    /api/tasks
POST   /api/tasks
GET    /api/tasks/{id}
PUT    /api/tasks/{id}
DELETE /api/tasks/{id}

## Modelle

### Job
{
  "id": "uuid",
  "name": "string",
  "description": "string",
  "isEnabled": boolean,
  "tasks": Task[],
  "created": "datetime",
  "modified": "datetime"
}

### Task
{
  "id": "uuid",
  "jobId": "uuid",
  "type": "string",
  "parameters": "json",
  "order": number,
  "isEnabled": boolean
}
