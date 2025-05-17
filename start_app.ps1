# start_app.ps1

Write-Host "Starting JTL TaskMaster Application..." -ForegroundColor Green

# 1. Überprüfe Voraussetzungen
Write-Host "Checking prerequisites..." -ForegroundColor Yellow

$prerequisites = @(
    @{
        Name = "Docker"
        Command = "docker --version"
    },
    @{
        Name = ".NET SDK"
        Command = "dotnet --version"
    },
    @{
        Name = "Node.js"
        Command = "node --version"
    }
)

foreach ($prereq in $prerequisites) {
    Write-Host "Checking $($prereq.Name)..." -NoNewline
    try {
        Invoke-Expression $prereq.Command | Out-Null
        Write-Host "OK" -ForegroundColor Green
    }
    catch {
        Write-Host "Missing!" -ForegroundColor Red
        Write-Host "Please install $($prereq.Name) to continue."
        exit 1
    }
}

# 2. Starte Docker-Container
Write-Host "`nStarting Docker containers..." -ForegroundColor Yellow
docker-compose -f infrastructure/docker/docker-compose.yml down
docker-compose -f infrastructure/docker/docker-compose.yml up -d

# Warte auf Datenbank
Write-Host "Waiting for database to be ready..." -NoNewline
Start-Sleep -Seconds 10
Write-Host "OK" -ForegroundColor Green

# 3. Führe Datenbank-Migrationen aus
Write-Host "`nApplying database migrations..." -ForegroundColor Yellow
Set-Location src/api/JTLTaskMaster.Api
dotnet ef database update
Set-Location ../../..

# 4. Starte Backend API
Write-Host "`nStarting Backend API..." -ForegroundColor Yellow
Start-Process -FilePath "dotnet" -ArgumentList "run --project src/api/JTLTaskMaster.Api/JTLTaskMaster.Api.csproj"

# 5. Starte Worker Service
Write-Host "Starting Worker Service..." -ForegroundColor Yellow
Start-Process -FilePath "dotnet" -ArgumentList "run --project src/worker/JTLTaskMaster.Worker.JobProcessor/JTLTaskMaster.Worker.JobProcessor.csproj"

# 6. Starte Frontend Development Server
Write-Host "Starting Frontend Development Server..." -ForegroundColor Yellow
Set-Location src/frontend
Start-Process -FilePath "npm" -ArgumentList "install"
Start-Process -FilePath "npm" -ArgumentList "run dev"
Set-Location ../..

# 7. Zeige Zugriffsinformationen
Write-Host "`nApplication started successfully!" -ForegroundColor Green
Write-Host "`nAccess the application at:"
Write-Host "Frontend: http://localhost:3000"
Write-Host "API: http://localhost:5000"
Write-Host "Swagger: http://localhost:5000/swagger"
Write-Host "Health Checks: http://localhost:5000/health"

# 8. Zeige Test-Zugangsdaten
Write-Host "`nTest credentials:"
Write-Host "Email: admin@jtltaskmaster.com"
Write-Host "Password: Admin123!"

# Optional: Öffne Browser
$openBrowser = Read-Host "`nWould you like to open the application in your browser? (y/n)"
if ($openBrowser -eq 'y') {
    Start-Process "http://localhost:3000"
}

Write-Host "`nPress Ctrl+C to stop all services." -ForegroundColor Yellow
