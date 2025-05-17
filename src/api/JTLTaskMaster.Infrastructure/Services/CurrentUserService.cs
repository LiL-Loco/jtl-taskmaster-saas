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
namespace JTLTaskMaster.Infrastructure.Services; public class CurrentUserService : ICurrentUserService { private readonly IHttpContextAccessor _httpContextAccessor; public CurrentUserService(IHttpContextAccessor httpContextAccessor) { _httpContextAccessor = httpContextAccessor; } public Guid UserId => Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? ""); public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name); public Guid TenantId => Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue("tenant_id") ?? ""); public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false; }
