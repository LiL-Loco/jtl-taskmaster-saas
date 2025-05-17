using System.Text.Json;

using JTLWawiExtern;

namespace JTLTaskMaster.Agent.Tasks.JtlWawiExternDll;

public class JtlWawiExternExecutor : ITaskExecutor
{
    private readonly ILogger<JtlWawiExternExecutor> _logger;
    private readonly JtlConfig _jtlConfig;
    private readonly string _dllPath;

    public JtlWawiExternExecutor(
        ILogger<JtlWawiExternExecutor> logger,
        IOptions<JtlConfig> jtlConfig)
    {
        _logger = logger;
        _jtlConfig = jtlConfig.Value;
        _dllPath = Path.Combine(_jtlConfig.WawiPath, "JTLWawiExtern.dll");
    }

    public async Task ExecuteAsync(
        string parameters,
        IProgress<int>? progress = null,
        CancellationToken cancellationToken = default)
    {
        var config = JsonSerializer.Deserialize<WawiExternParameters>(parameters)
            ?? throw new ArgumentException("Invalid parameters");

        if (!File.Exists(_dllPath))
        {
            throw new FileNotFoundException("JTLWawiExtern.dll not found", _dllPath);
        }

        progress?.Report(10);

        try
        {
            bool result = false;

            switch (config.Action)
            {
                case "ImportVersanddaten":
                    result = ImportVersanddaten(config.Parameters, progress);
                    break;

                case "ExecuteWorkflow":
                    result = await ExecuteWorkflow(config.Parameters, progress);
                    break;

                default:
                    _logger.LogError($"Unknown action: {config.Action}");
                    break;
            }

            if (!result)
            {
                _logger.LogWarning("Task execution completed but returned false");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing JTL task");
        }
    }

    private bool ImportVersanddaten(string parameters, IProgress<int>? progress)
    {
        var importParams = JsonSerializer.Deserialize<VersanddatenParameters>(parameters)
            ?? throw new ArgumentException("Invalid Versanddaten parameters");

        progress?.Report(30);

        var importer = new VersanddatenImport();
        var result = importer.ImportVersanddaten(
            importParams.Lieferscheinnummer,
            importParams.Trackingcode,
            importParams.Versanddatum,
            importParams.Versanddienstleister);

        progress?.Report(70);

        if (!result.Success)
        {
            throw new Exception($"Versanddaten import failed: {result.Message}");
        }

        _logger.LogInformation("Successfully imported Versanddaten for Lieferschein {Lieferschein}",
            importParams.Lieferscheinnummer);

        return true;
    }

    private async Task<bool> ExecuteWorkflow(string parameters, IProgress<int>? progress)
    {
        var workflowParams = JsonSerializer.Deserialize<WorkflowParameters>(parameters)
            ?? throw new ArgumentException("Invalid Workflow parameters");

        progress?.Report(30);

        var workflow = new WorkflowExecution();
        var result = workflow.ExecuteWorkflow(
            workflowParams.WorkflowId,
            workflowParams.Parameters);

        progress?.Report(70);

        if (!result.Success)
        {
            throw new Exception($"Workflow execution failed: {result.Message}");
        }

        _logger.LogInformation("Successfully executed workflow {WorkflowId}",
            workflowParams.WorkflowId);

        return true;
    }
}

public class WawiExternParameters
{
    public string Action { get; set; } = string.Empty;
    public string Parameters { get; set; } = string.Empty;
}

public class VersanddatenParameters
{
    public string Lieferscheinnummer { get; set; } = string.Empty;
    public string Trackingcode { get; set; } = string.Empty;
    public DateTime Versanddatum { get; set; }
    public string Versanddienstleister { get; set; } = string.Empty;
}

public class WorkflowParameters
{
    public string WorkflowId { get; set; } = string.Empty;
    public string Parameters { get; set; } = string.Empty;
}