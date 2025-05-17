using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JTLTaskMaster.Application.Common.Interfaces;

namespace JTLTaskMaster.Api.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class MonitoringController : ControllerBase
{
    private readonly IJobService _jobService;
    private readonly IAgentRegistry _agentRegistry;
    private readonly IMetricsService _metricsService;

    public MonitoringController(
        IJobService jobService,
        IAgentRegistry agentRegistry,
        IMetricsService metricsService)
    {
        _jobService = jobService;
        _agentRegistry = agentRegistry;
        _metricsService = metricsService;
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardStats>> GetDashboardStats()
    {
        var stats = new DashboardStats
        {
            TotalJobs = await _jobService.GetTotalJobCount(),
            ActiveJobs = await _jobService.GetActiveJobCount(),
            FailedJobs = await _jobService.GetFailedJobCount(),
            ConnectedAgents = await _agentRegistry.GetConnectedAgentCount(),
            TotalAgents = await _agentRegistry.GetTotalAgentCount()
        };

        return Ok(stats);
    }

    [HttpGet("agents")]
    public async Task<ActionResult<List<AgentStatus>>> GetAgentStatus()
    {
        var agents = await _agentRegistry.GetAllAgentsAsync();
        return Ok(agents);
    }

    [HttpGet("jobs/failed")]
    public async Task<ActionResult<List<FailedJobInfo>>> GetFailedJobs([FromQuery] int limit = 10)
    {
        var failedJobs = await _jobService.GetRecentFailedJobs(limit);
        return Ok(failedJobs);
    }

    [HttpGet("jobs/{id}/history")]
    public async Task<ActionResult<JobExecutionHistory>> GetJobHistory(Guid id)
    {
        var history = await _jobService.GetJobExecutionHistory(id);
        if (history == null) return NotFound();
        return Ok(history);
    }
}

public class DashboardStats
{
    public int TotalJobs { get; set; }
    public int ActiveJobs { get; set; }
    public int FailedJobs { get; set; }
    public int ConnectedAgents { get; set; }
    public int TotalAgents { get; set; }
}