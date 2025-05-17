using Microsoft.Extensions.Diagnostics.HealthChecks;
using JTLTaskMaster.Application.Common.Interfaces;

namespace JTLTaskMaster.Api.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IApplicationDbContext _context;

    public DatabaseHealthCheck(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Try to connect to database
            var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
            if (!canConnect)
            {
                return HealthCheckResult.Unhealthy("Cannot connect to database");
            }

            // Check if there are any pending migrations
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync(cancellationToken);
            if (pendingMigrations.Any())
            {
                return HealthCheckResult.Degraded($"Database has {pendingMigrations.Count()} pending migrations");
            }

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database health check failed", ex);
        }
    }
}

public class RedisHealthCheck : IHealthCheck
{
    private readonly IConnectionMultiplexer _redis;

    public RedisHealthCheck(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var database = _redis.GetDatabase();
            var ping = database.PingAsync().Result;

            return Task.FromResult(HealthCheckResult.Healthy($"Redis response time: {ping.TotalMilliseconds}ms"));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("Redis health check failed", ex));
        }
    }
}

public class AgentConnectionHealthCheck : IHealthCheck
{
    private readonly IAgentRegistry _agentRegistry;

    public AgentConnectionHealthCheck(IAgentRegistry agentRegistry)
    {
        _agentRegistry = agentRegistry;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var agents = await _agentRegistry.GetAllAgentsAsync(cancellationToken);
            var connectedAgents = agents.Count(a => a.IsConnected);
            var totalAgents = agents.Count;

            if (connectedAgents == 0 && totalAgents > 0)
            {
                return HealthCheckResult.Unhealthy($"No agents connected. Total registered: {totalAgents}");
            }

            if (connectedAgents < totalAgents)
            {
                return HealthCheckResult.Degraded(
                    $"Some agents are disconnected. Connected: {connectedAgents}/{totalAgents}");
            }

            return HealthCheckResult.Healthy($"All agents connected ({connectedAgents})");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Agent health check failed", ex);
        }
    }
}