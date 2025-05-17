# Projektplan: JTL TaskMaster (SaaS)

## 1. Projektübersicht

*   **Projektname:** JTL TaskMaster (SaaS)
*   **Ziel:** Entwicklung einer mandantenfähigen SaaS-Plattform zur Automatisierung von Aufgaben im JTL-Wawi-Umfeld und allgemeinen Dateioperationen. Die Plattform soll über eine Web-Oberfläche bedienbar sein und einen lokalen Agenten für die Interaktion mit lokalen Kundenressourcen nutzen.
*   **Kernzielgruppe:** Unternehmen, die JTL-Wawi einsetzen und wiederkehrende Prozesse automatisieren möchten.
*   **Monetarisierung:** Abonnementbasiertes Modell (z.B. gestaffelt nach Funktionsumfang, Anzahl Jobs, Anzahl Agenten).

## 2. Kernfunktionen (MVP - Minimum Viable Product)

*   **Benutzerverwaltung & Authentifizierung:** Registrierung, Login, Passwort-Reset, Rollen (Admin, Benutzer).
*   **Mandantenfähigkeit:** Strikte Trennung der Daten verschiedener Kunden.
*   **Job-Management (Web-UI):**
    *   Erstellen, Bearbeiten, Löschen, Aktivieren/Deaktivieren von Jobs.
    *   Definition von Tasks innerhalb eines Jobs (sequenzielle Ausführung).
*   **Konfigurationsmanagement (Web-UI):**
    *   Verwaltung von E-Mail-Konten (SMTP).
    *   Verwaltung von FTP-Server-Profilen.
    *   Verwaltung von JTL-Agenten-Verbindungen.
*   **Lokaler JTL-Agent:**
    *   Sichere Kommunikation mit der SaaS-Plattform.
    *   Abruf von auszuführenden Tasks.
    *   Ausführung folgender Tasks (lokal beim Kunden):
        *   JTL-Ameise - Import/Export
        *   JTLWawiExtern.dll: Versanddaten importieren
        *   JTLWawiExtern.dll: Workflows auslösen
        *   Dateitransfer: Download von FTP-Server / Upload zu FTP Server (ggf. auch lokale FTP-Server des Kunden)
        *   Sonstiges: Datei aus Web laden
        *   Sonstiges: Datei aus Verzeichnis laden / Daten in Verzeichnis speichern (lokal)
        *   Sonstiges: E-Mail senden (über konfigurierte SMTP-Konten)
        *   Sonstiges: Prozess starten (lokal)
        *   Sonstiges: Daten von MS-SQL Server laden (lokaler oder erreichbarer MS-SQL des Kunden)
    *   Übermittlung von Status und Logs an die SaaS-Plattform.
*   **Worker-Dienst (SaaS-Backend):**
    *   Verwaltung der Job-Warteschlange.
    *   Dispatching von Tasks an den entsprechenden lokalen Agenten.
    *   Verarbeitung von Status-Updates und Logs vom Agenten.
*   **Logging & Monitoring (Web-UI):**
    *   Anzeige von Job-Ausführungslogs.
    *   Status der Agenten.

## 3. Technologie-Stack (Vorschlag)

*   **Frontend:** React, Vue.js, oder Angular (z.B. React mit TypeScript)
*   **Backend API:** .NET 8 (C#) mit ASP.NET Core Web API
*   **Worker-Dienste:** .NET 8 (C#) (z.B. als gehostete Dienste in ASP.NET Core oder separate Worker-Prozesse)
*   **Datenbank:** PostgreSQL (robust, skalierbar, gut für relationale Daten und JSON)
*   **Lokaler Agent:** .NET 8 (C#) (als Windows Service oder Konsolenanwendung)
*   **Echtzeitkommunikation (Agent <-> Backend):** SignalR (für .NET-Stack) oder WebSockets / gRPC.
*   **Nachrichtenwarteschlange (für Worker):** RabbitMQ oder Azure Service Bus / AWS SQS (optional für höhere Skalierbarkeit)
*   **Caching:** Redis (optional für Performance-Optimierung)
*   **Infrastruktur:** Linux-Server mit Nginx (als Reverse Proxy für die API), Docker für Containerisierung.
*   **Authentifizierung:** JWT (JSON Web Tokens) mit Identity-Lösung (z.B. ASP.NET Core Identity, Keycloak, Auth0).

## 4. Entwicklungsphasen & Sprints

### Phase 1: Fundament & Kern-API (ca. 4-6 Wochen)
*   **Sprint 0: Setup & Architektur**
    *   Projektstruktur, Repositories (Git).
    *   CI/CD-Grundlagen.
    *   Basis-Architekturentscheidungen finalisieren.
    *   Docker-Setup für lokale Entwicklung.
*   **Sprint 1: Authentifizierung & Benutzerverwaltung**
    *   API-Endpunkte für Registrierung, Login, Profil.
    *   Datenbankmodelle für Benutzer, Mandanten.
    *   JWT-Implementierung.
*   **Sprint 2: Basis Job & Task API**
    *   API-Endpunkte für CRUD-Operationen für Jobs und Tasks (ohne Ausführungslogik).
    *   Datenbankmodelle für Jobs, Tasks, Parameter.
    *   Mandantentrennung für Job-Daten.

### Phase 2: Frontend-Grundgerüst & Agenten-Prototyp (ca. 6-8 Wochen)
*   **Sprint 3: Frontend Setup & Login**
    *   Frontend-Projekt aufsetzen.
    *   Login-, Registrierungs-Seiten.
    *   Dashboard-Grundgerüst.
*   **Sprint 4: Job-Management UI (Basis)**
    *   Anzeige von Jobs.
    *   Formulare zum Erstellen/Bearbeiten von Jobs und Tasks (Parameter-UI noch rudimentär).
*   **Sprint 5: Lokaler Agent - Prototyp & Kommunikation**
    *   Basis-Agent-Anwendung (.NET).
    *   Sichere Registrierung/Authentifizierung des Agenten bei der API.
    *   Implementierung der Echtzeitkommunikation (z.B. SignalR Client im Agent, Server in API).
    *   Agent pollt/empfängt Test-Tasks von der API.
*   **Sprint 6: Agent - Basisfunktion "Prozess starten"**
    *   Implementierung des "Prozess starten"-Tasks im Agenten.
    *   Status-Rückmeldung an die API.
    *   Basis-Logging vom Agenten zur API.

### Phase 3: Worker-Dienst & Kern-Task-Implementierungen (ca. 8-12 Wochen)
*   **Sprint 7: Worker-Dienst & Job-Dispatching**
    *   Entwicklung des Backend-Worker-Dienstes.
    *   Job-Queue-Mechanismus (initial In-Memory oder einfache DB-Tabelle, später ggf. RabbitMQ).
    *   Dispatching von Tasks an den registrierten, passenden Agenten.
*   **Sprint 8: JTL Ameise & DLL-Integration im Agenten**
    *   Implementierung der Tasks: JTL-Ameise Import/Export, JTLWawiExtern.dll Versanddaten/Workflows.
    *   Umgang mit Pfaden und Konfigurationen für Ameise/DLL im Agenten (ggf. vom Server übermittelt oder lokal konfiguriert).
*   **Sprint 9: FTP & Dateisystem-Tasks im Agenten**
    *   Implementierung der FTP-Tasks und lokaler Dateioperationen.
*   **Sprint 10: E-Mail & SQL-Tasks**
    *   Implementierung E-Mail-Senden (über API-konfigurierte Konten, ausgeführt vom Agenten oder Backend).
    *   Implementierung MS-SQL-Daten laden (ausgeführt vom Agenten).
*   **Sprint 11: Konfigurations-UI (FTP, Mail)**
    *   Frontend-UI zur Verwaltung von FTP-Servern und E-Mail-Konten.
    *   API-Endpunkte dafür.

### Phase 4: Erweiterte Funktionen & Stabilisierung (ca. 6-10 Wochen)
*   **Sprint 12: Logging-Anzeige & Agenten-Monitoring UI**
    *   Detaillierte Job-Log-Anzeige im Frontend.
    *   Übersicht und Status der registrierten Agenten.
*   **Sprint 13: UI-Verbesserungen & Task-Parameter-Editor**
    *   Verbesserung der Benutzerfreundlichkeit.
    *   Dynamischer und typsicherer Editor für Task-Parameter im Frontend.
*   **Sprint 14: JTLWawiExtern.dll - Erweiterte Funktionen**
    *   Implementierung XML-Importe, Drucken, Dateien speichern via DLL im Agenten (falls relevant).
*   **Sprint 15: Sicherheit & Performance-Optimierung**
    *   Security Audit (OWASP Top 10).
    *   Performance-Tests und Optimierungen.
    *   Fehlerbehandlung und Robustheit verbessern.
*   **Sprint 16: Testing, Bugfixing & Dokumentation**
    *   Umfassende End-to-End-Tests.
    *   Erstellung von Benutzer- und Admin-Dokumentation.

### Phase 5: Beta & Launch (laufend)
*   Interne Tests, geschlossene Beta-Phase mit ausgewählten Kunden.
*   Feedback einarbeiten.
*   Vorbereitung für den Launch (Marketing, Support-Strukturen).
*   Deployment auf Produktivsystem.

## 5. Wichtige Überlegungen

*   **Sicherheit:** Höchste Priorität. HTTPS überall, sichere Speicherung von Credentials, Schutz vor gängigen Web-Angriffen, sichere Agenten-Kommunikation.
*   **Multi-Tenancy:** Sorgfältiges Design der Datenbank und Anwendungslogik zur strikten Datentrennung.
*   **Skalierbarkeit:** Architektur so gestalten, dass einzelne Komponenten (API, Worker, Datenbank) bei Bedarf skaliert werden können.
*   **Lokaler Agent:**
    *   **Sicherheit:** Der Agent hat Zugriff auf lokale Systeme des Kunden. Die Kommunikation muss absolut sicher sein.
    *   **Installation & Updates:** Einfacher Installationsprozess und eine Strategie für automatische Updates des Agenten.
    *   **Robustheit:** Der Agent muss auch bei Netzwerkproblemen stabil laufen und sich wieder verbinden können.
*   **Fehlerbehandlung & Logging:** Umfassend und über alle Komponenten hinweg.
*   **DSGVO-Konformität:** Von Anfang an berücksichtigen.
*   **Kosten:** Entwicklungskosten, laufende Kosten für Infrastruktur, Wartung.

## 6. Team (Beispiel)

*   1-2 Backend-Entwickler (.NET Core)
*   1 Frontend-Entwickler (React/Vue/Angular)
*   1 DevOps/Systemadministrator (Teilzeit oder nach Bedarf)
*   1 Produktmanager/Tester

Dieser Plan ist ein Entwurf und muss im Laufe des Projekts angepasst werden. Die Zeitangaben sind grobe Schätzungen für ein erfahrenes Team.

# Ordnerstruktur: JTL TaskMaster (SaaS)

Dies ist eine vorgeschlagene Ordnerstruktur für das Gesamtprojekt. Sie kann je nach spezifischen Entscheidungen und Framework-Konventionen angepasst werden.

# Projektstruktur JTLTaskMaster

## /docs
- /architecture
- /api
- /deployment
- /development
- /database
- /security
- /testing

## /infrastructure
- /docker
  - /api
  - /frontend
  - /worker
  - /db
  - /nginx
- /nginx
- /terraform

## /src
- /api
  - /JTLTaskMaster.Api
    - /Controllers
    - /Models
    - /Services
    - /Infrastructure
    - /Auth
    - /Hubs
    - /Properties
- /worker
  - /JTLTaskMaster.Worker
    - /Services
    - /Tasks
- /agent
  - /JTLTaskMaster.Agent
    - /Services
    - /Tasks
      - /JtlAmeise
      - /JtlWawiExternDll
    - /Config
- /shared
  - /JTLTaskMaster.Shared
    - /Models
    - /Constants
- /frontend
  - /public
  - /src
    - /components
    - /pages
    - /services
    - /contexts
    - /hooks
    - /assets

## /tests
- /api.tests
  - /JTLTaskMaster.Api.UnitTests
  - /JTLTaskMaster.Api.IntegrationTests
- /frontend.tests
- /worker.tests
- /agent.tests

## Root Files
- .gitignore
- README.md
- docker-compose.yml
- JTLTaskMaster.sln

**Hinweise zur Struktur:**

*   **Monorepo vs. Polyrepo:** Diese Struktur deutet auf ein Monorepo hin (aller Code in einem Repository). Dies kann Vorteile bei der Verwaltung von Abhängigkeiten und der gemeinsamen Entwicklung haben. Alternativ könnten API, Frontend, Worker und Agent auch in separaten Repositories liegen (Polyrepo).
*   **Framework-Konventionen:** Die genaue Struktur innerhalb von `api/`, `frontend/` etc. wird stark von den gewählten Frameworks (ASP.NET Core, React etc.) und deren Best Practices beeinflusst.
*   **Shared Library:** Eine `shared` Library ist sehr nützlich, um Datenmodelle (DTOs), Enums oder Hilfsfunktionen zu teilen, die sowohl vom Backend als auch vom Agenten (und ggf. vom Worker) benötigt werden, um Inkonsistenzen zu vermeiden.

# Setup der Entwicklungsumgebung: JTL TaskMaster (SaaS)

Diese Anleitung beschreibt die notwendigen Werkzeuge und Schritte, um eine lokale Entwicklungsumgebung für das JTL TaskMaster SaaS-Projekt aufzusetzen.

## 1. Grundlegende Werkzeuge (Systemweit)

*   **Git:** Zur Versionskontrolle. [https://git-scm.com/](https://git-scm.com/)
*   **Docker Desktop:** Zur Containerisierung von Diensten wie Datenbank, Nginx, und ggf. der Anwendungsteile für eine produktionsnahe Entwicklung. [https://www.docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop)
*   **IDE(s):**
    *   **Für .NET (Backend, Worker, Agent):**
        *   Visual Studio 2022 (Community Edition ist kostenlos) [https://visualstudio.microsoft.com/](https://visualstudio.microsoft.com/)
        *   JetBrains Rider (kostenpflichtig, aber sehr leistungsstark)
        *   Visual Studio Code mit C# Dev Kit Extension
    *   **Für Frontend (z.B. React/Vue/Angular):**
        *   Visual Studio Code (empfohlen, kostenlos) [https://code.visualstudio.com/](https://code.visualstudio.com/)
        *   WebStorm (kostenpflichtig, von JetBrains)
*   **Node.js und npm/yarn:** Für Frontend-Entwicklung und Build-Prozesse. Installieren Sie die aktuelle LTS-Version. [https://nodejs.org/](https://nodejs.org/)
    *   `npm install -g yarn` (optional, wenn yarn bevorzugt wird)
*   **.NET SDK:** Installieren Sie das .NET 8 SDK (oder die gewählte Version). [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
*   **Postman oder Insomnia:** Zum Testen der Backend-API-Endpunkte.

## 2. Projektspezifisches Setup

### 2.1. Klonen des Repositorys
```bash
git clone <repository-url> jtl-taskmaster-saas
cd jtl-taskmaster-saas

