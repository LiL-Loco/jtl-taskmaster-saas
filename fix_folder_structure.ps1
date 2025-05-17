# fix_build_errors_v2.ps1

# 1. Korrigiere CurrentUserService.cs
$currentUserServiceContent = @"
using JTLTaskMaster.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JTLTaskMaster.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId => Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
    public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
    public Guid TenantId => Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue("tenant_id") ?? "");
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
"@
Set-Content "src/api/JTLTaskMaster.Infrastructure/Services/CurrentUserService.cs" -Value $currentUserServiceContent

# 2. Füge die using-Direktive in SeedData.cs hinzu
$seedDataContent = Get-Content "src/api/JTLTaskMaster.Persistence/SeedData.cs"
if ($seedDataContent -notmatch "using JTLTaskMaster\.Persistence;") {
    $seedDataContent = "using JTLTaskMaster.Persistence;`n" + $seedDataContent
    Set-Content "src/api/JTLTaskMaster.Persistence/SeedData.cs" -Value $seedDataContent
}

# 3. Füge RetryCount zu JobTask hinzu
$jobTaskContent = Get-Content "src/api/JTLTaskMaster.Domain/Entities/JobTask.cs"
if ($jobTaskContent -notmatch "RetryCount") {
    $jobTaskContent = $jobTaskContent -replace "public Guid TenantId { get; set; }", "public Guid TenantId { get; set; }`n public int RetryCount { get; set; }"
    Set-Content "src/api/JTLTaskMaster.Domain/Entities/JobTask.cs" -Value $jobTaskContent
}

# 4. Implementiere GetAvailableAgentForTask in IAgentService und AgentService
#    (Hier müssen Sie die Logik implementieren, wie ein Agent für einen Task gefunden wird)

# 5. Korrigiere using-Direktiven in Domain-Projekt
$jobContent = Get-Content "src/api/JTLTaskMaster.Domain/Entities/Job.cs"
$jobContent = $jobContent -replace "using JTLTaskMaster\.Domain\.Enums;", ""
Set-Content "src/api/JTLTaskMaster.Domain/Entities/Job.cs" -Value $jobContent

$jobTaskContent = Get-Content "src/api/JTLTaskMaster.Domain/Entities/JobTask.cs"
$jobTaskContent = $jobTaskContent -replace "using JTLTaskMaster\.Domain\.Enums;", ""
Set-Content "src/api/JTLTaskMaster.Domain/Entities/JobTask.cs" -Value $jobTaskContent

# 6. Bereinige und baue die Solution
dotnet clean
dotnet restore
dotnet build

Write-Host "Skript wurde ausgeführt. Überprüfe den Build auf verbleibende Fehler."
