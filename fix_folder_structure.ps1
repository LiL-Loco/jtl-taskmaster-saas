# fix_ambiguous_references.ps1

# 1. Füge using MediatR; zu den betroffenen Dateien hinzu
$filesToAddMediatRUsing = @(
    "src/api/JTLTaskMaster.Application/Features/Jobs/Commands/CreateJob/CreateJobCommand.cs",
    "src/api/JTLTaskMaster.Application/Features/Jobs/Commands/StartJob/StartJobCommand.cs"
)

foreach ($file in $filesToAddMediatRUsing) {
    $content = Get-Content $file
    if ($content -notmatch "^using MediatR;") {
        $content = "using MediatR;`n" + $content
        Set-Content $file -Value $content
    }
}

# 2. Verwende Aliase oder vollqualifizierte Namen, um Mehrdeutigkeiten zu vermeiden
#    Beispiel für CreateJobCommand.cs:
$createJobCommandContent = Get-Content "src/api/JTLTaskMaster.Application/Features/Jobs/Commands/CreateJob/CreateJobCommand.cs"
$createJobCommandContent = $createJobCommandContent -replace "IRequest<", "MediatR.IRequest<"
$createJobCommandContent = $createJobCommandContent -replace "IRequestHandler<", "MediatR.IRequestHandler<"
Set-Content "src/api/JTLTaskMaster.Application/Features/Jobs/Commands/CreateJob/CreateJobCommand.cs" -Value $createJobCommandContent

#    Wiederholen Sie diesen Schritt für alle betroffenen Dateien und Interfaces.

# 3. Füge die using-Direktive für ICurrentUserService in CreateJobCommand.cs hinzu
$createJobCommandContent = Get-Content "src/api/JTLTaskMaster.Application/Features/Jobs/Commands/CreateJob/CreateJobCommand.cs"
if ($createJobCommandContent -notmatch "using JTLTaskMaster\.Application\.Common\.Interfaces;") {
    $createJobCommandContent = "using JTLTaskMaster.Application.Common.Interfaces;`n" + $createJobCommandContent
    Set-Content "src/api/JTLTaskMaster.Application/Features/Jobs/Commands/CreateJob/CreateJobCommand.cs" -Value $createJobCommandContent
}

# 4. Füge fehlende using-Direktiven in Infrastructure hinzu
$infrastructureUsings = @"
using JTLTaskMaster.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using JTLTaskMaster.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
"@

foreach ($file in Get-ChildItem "src/api/JTLTaskMaster.Infrastructure/**/*.cs" -Recurse) {
    $content = Get-Content $file.FullName
    if ($content -notmatch $infrastructureUsings) {
        $content = $infrastructureUsings + $content
        Set-Content $file.FullName -Value $content
    }
}

# 5. Bereinige und baue die Solution
dotnet clean
dotnet restore
dotnet build

Write-Host "Skript wurde ausgeführt. Überprüfe den Build auf verbleibende Fehler."
