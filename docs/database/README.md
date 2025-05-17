# Datenbank Dokumentation

## Schema

### Tabellen

#### Users
CREATE TABLE Users (
    Id UUID PRIMARY KEY,
    Email VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    TenantId UUID NOT NULL,
    CreatedAt TIMESTAMP NOT NULL,
    ModifiedAt TIMESTAMP NOT NULL
);

#### Jobs
CREATE TABLE Jobs (
    Id UUID PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Description TEXT,
    IsEnabled BOOLEAN NOT NULL DEFAULT true,
    TenantId UUID NOT NULL,
    CreatedAt TIMESTAMP NOT NULL,
    ModifiedAt TIMESTAMP NOT NULL
);

## Migrations
Migrations werden mit Entity Framework Core verwaltet:

# Migration erstellen
dotnet ef migrations add InitialCreate

# Migration anwenden
dotnet ef database update

## Backup & Recovery
# Backup erstellen
pg_dump -U postgres jtl_taskmaster > backup.sql

# Backup einspielen
psql -U postgres jtl_taskmaster < backup.sql
