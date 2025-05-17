using JTLTaskMaster.Worker.JobProcessor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Configure options
builder.Services.Configure<JobProcessingOptions>(builder.Configuration.GetSection("JobProcessing"));

// Add the job processor as a hosted service
builder.Services.AddHostedService<JobProcessor>();

// Build and run the host
var host = builder.Build();
host.Run();