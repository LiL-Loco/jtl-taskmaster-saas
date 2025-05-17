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

namespace JTLTaskMaster.Infrastructure.Services;

public class TelemetryService : ITelemetryService
{
    private readonly TracerProvider _tracerProvider; private readonly MeterProvider _meterProvider; private readonly ILogger<TelemetryService> _logger; public TelemetryService(IConfiguration configuration, ILogger<TelemetryService> logger)
    {
        _logger = logger;          // Configure OpenTelemetry tracing        
        _tracerProvider = Sdk.CreateTracerProviderBuilder()
        .AddSource("JTLTaskMaster").AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddEntityFrameworkCoreInstrumentation().AddOtlpExporter(options => { options.Endpoint = new Uri(configuration["Telemetry:OtlpEndpoint"]!); }).Build();          // Configure OpenTelemetry metrics         
        _meterProvider = Sdk.CreateMeterProviderBuilder().AddMeter("JTLTaskMaster").AddAspNetCoreInstrumentation().AddRuntimeInstrumentation().AddOtlpExporter(options => { options.Endpoint = new Uri(configuration["Telemetry:OtlpEndpoint"]!); }).Build();
    }
    public IDisposable StartOperation(string name, string? context = null) { var activity = System.Diagnostics.Activity.Current; if (activity != null) { activity.SetTag("operation", name); if (context != null) { activity.SetTag("context", context); } } return new OperationScope(name, _logger); }
}
internal class OperationScope : IDisposable
{
    private readonly string _operationName; private readonly ILogger _logger; private readonly System.Diagnostics.Stopwatch _stopwatch; public OperationScope(string operationName, ILogger logger) { _operationName = operationName; _logger = logger; _stopwatch = System.Diagnostics.Stopwatch.StartNew(); _logger.LogInformation("Starting operation: {Operation}", _operationName); }
    public void Dispose()
    {
        _stopwatch.Stop(); _logger.LogInformation("Completed operation: {Operation}, Duration: {Duration}ms", _operationName, _stopwatch.ElapsedMilliseconds);
    }
}
