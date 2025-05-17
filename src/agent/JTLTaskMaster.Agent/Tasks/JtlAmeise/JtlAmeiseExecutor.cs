using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using JTLTaskMaster.Agent.Config;
using JTLTaskMaster.Agent.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JTLTaskMaster.Agent.Tasks.JtlAmeise;

public class JtlAmeiseExecutor : ITaskExecutor
{
    private readonly ILogger<JtlAmeiseExecutor> _logger;
    private readonly JtlConfig _jtlConfig;

    public JtlAmeiseExecutor(
        ILogger<JtlAmeiseExecutor> logger,
        IOptions<JtlConfig> jtlConfig)
    {
        _logger = logger;
        _jtlConfig = jtlConfig.Value;
    }

    public async Task ExecuteAsync(
        string parameters,
        IProgress<int>? progress,
        CancellationToken cancellationToken)
    {
        var config = JsonSerializer.Deserialize<AmeiseParameters>(parameters)
            ?? throw new ArgumentException("Invalid parameters");

        var ameisePath = Path.Combine(_jtlConfig.WawiPath, "Ameise.exe");
        if (!File.Exists(ameisePath))
        {
            throw new FileNotFoundException("Ameise.exe not found", ameisePath);
        }

        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = ameisePath,
                Arguments = BuildArguments(config),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        var output = new StringBuilder();
        var error = new StringBuilder();

        process.OutputDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                output.AppendLine(e.Data);
                UpdateProgress(e.Data, progress);
            }
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                error.AppendLine(e.Data);
            }
        };

        _logger.LogInformation("Starting Ameise process: {Arguments}",
            process.StartInfo.Arguments);

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        using var registration = cancellationToken.Register(() =>
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error killing Ameise process");
            }
        });

        await process.WaitForExitAsync(cancellationToken);

        if (process.ExitCode != 0)
        {
            throw new TaskExecutionException(
                "Ameise process failed with exit code: " + process.ExitCode +
                "\nError: " + error.ToString());
        }

        _logger.LogInformation("Ameise process completed successfully");
    }

    private string BuildArguments(AmeiseParameters config)
    {
        var args = new List<string>
        {
            $"/template \"{config.TemplatePath}\"",
            $"/mode {(config.IsImport ? "import" : "export")}"
        };

        if (!string.IsNullOrEmpty(config.InputFile))
        {
            args.Add($"/input \"{config.InputFile}\"");
        }

        if (!string.IsNullOrEmpty(config.OutputFile))
        {
            args.Add($"/output \"{config.OutputFile}\"");
        }

        return string.Join(" ", args);
    }

    private void UpdateProgress(string line, IProgress<int>? progress)
    {
        if (progress == null) return;

        // Beispiel: Parse Ameise Output für Fortschritt
        if (line.Contains("Progress:"))
        {
            var percentStr = line.Split(':')[1].Trim().TrimEnd('%');
            if (int.TryParse(percentStr, out var percent))
            {
                progress.Report(percent);
            }
        }
    }
}

public class AmeiseParameters
{
    public string TemplatePath { get; set; } = string.Empty;
    public bool IsImport { get; set; }
    public string? InputFile { get; set; }
    public string? OutputFile { get; set; }
}