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
using Microsoft.Extensions.Configuration;using StackExchange.Redis;  
namespace JTLTaskMaster.Infrastructure.Services;  public class JobLockManager
{
    private readonly IConnectionMultiplexer _redis;
    private readonly TimeSpan _lockTimeout = TimeSpan.FromMinutes(30);
    // Maximum lock duration     

    private readonly ILogger<JobLockManager> _logger;
    public JobLockManager(IConnectionMultiplexer redis, ILogger<JobLockManager> logger) { _redis = redis; _logger = logger; }
    public async Task<bool> TryAcquireLock(Guid jobId) { var db = _redis.GetDatabase(); var lockKey = GetLockKey(jobId);
        // Try to acquire lock with expiration         
    var acquired = await db.StringSetAsync(
        lockKey,
        "1",
        _lockTimeout,
        When.NotExists);
        if (acquired) { _logger.LogInformation("Acquired lock for job {JobId}", jobId); } else { _logger.LogWarning("Failed to acquire lock for job {JobId}", jobId); }          return acquired;     }      public async Task ReleaseLock(Guid jobId)     {         var db = _redis.GetDatabase();         var lockKey = GetLockKey(jobId);                  await db.KeyDeleteAsync(lockKey);         _logger.LogInformation("Released lock for job {JobId}", jobId);     }      private static string GetLockKey(Guid jobId) => $"job-lock:{jobId}"; 
    }
