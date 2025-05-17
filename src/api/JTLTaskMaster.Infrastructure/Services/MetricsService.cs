using System.Diagnostics.Metrics;
using JTLTaskMaster.Application.Common.Interfaces;

namespace JTLTaskMaster.Infrastructure.Services;

public class MetricsService : IMetricsService
{
    private readonly Meter _meter;
    private readonly Counter<int> _jobStartedCounter;
    private readonly Counter<int> _jobCompletedCounter;
    private readonly Counter<int> _jobFailedCounter;
    private readonly Histogram<double> _jobDurationHistogram;
    private readonly ObservableGauge<int> _activeJobsGauge;
    private readonly ObservableGauge<int> _connectedAgentsGauge;

    private readonly IApplicationDbContext _context;
    private readonly IAgentRegistry _agentRegistry;

    public MetricsService(
        IApplicationDbContext context,
        IAgentRegistry agentRegistry)
    {
        _context = context;
        _agentRegistry = agentRegistry;

        _meter = new Meter("JTLTaskMaster");

        _jobStartedCounter = _meter.CreateCounter<int>("jobs.started");
        _jobCompletedCounter = _meter.CreateCounter<int>("jobs.completed");
        _jobFailedCounter = _meter.CreateCounter<int>("jobs.failed");
        
        _jobDurationHistogram = _meter.CreateHistogram<double>(
            "job.duration",
            unit: "s",
            description: "Job execution duration in seconds");

        _activeJobsGauge = _meter.CreateObservableGauge<int>(
            "jobs.active",
            () => _context.Jobs.Count(j => j.Status == JobStatus.Running));

        _connectedAgentsGauge = _meter.CreateObservableGauge<int>(
            "agents.connected",
            () => _agentRegistry.GetConnectedAgentCount());
    }

    public void JobStarted(Guid jobId)
    {
        _jobStartedCounter.Add(1, new KeyValuePair<string, object?>("job_id", jobId));
    }

    public void JobCompleted(Guid jobId, TimeSpan duration)
    {
        _jobCompletedCounter.Add(1, new KeyValuePair<string, object?>("job_id", jobId));
        _jobDurationHistogram.Record(duration.TotalSeconds);
    }

    public void JobFailed(Guid jobId, string reason)
    {
        _jobFailedCounter.Add(1, new KeyValuePair<string, object?>("job_id", jobId),
            new KeyValuePair<string, object?>("reason", reason));
    }
}