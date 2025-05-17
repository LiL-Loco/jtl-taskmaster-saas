using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using JTLTaskMaster.Agent.Services;
using JTLTaskMaster.Agent.Tasks;
using JTLTaskMaster.Agent.Config;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // Register configuration options
        services.Configure<AgentConfig>(hostContext.Configuration.GetSection("AgentConfig"));
        
        // Register services
        services.AddSingleton<ITaskExecutorFactory, TaskExecutorFactory>();
        
        // Register the background service
        services.AddHostedService<AgentService>();
    });

var host = builder.Build();

await host.RunAsync();