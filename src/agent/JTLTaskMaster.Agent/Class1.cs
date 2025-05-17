// In Agent Projekt:
using Microsoft.Extensions.Hosting;

public class AgentService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Agent startup logic
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Agent cleanup logic
        return Task.CompletedTask;
    }
}