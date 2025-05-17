using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JTLTaskMaster.Api.Controllers.Base;

namespace JTLTaskMaster.Api.Controllers;

[Authorize]
public class AgentController : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<AgentRegistrationResponse>> RegisterAgent(RegisterAgentCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost("heartbeat")]
    public async Task<ActionResult> Heartbeat(AgentHeartbeatCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpGet("tasks")]
    public async Task<ActionResult<List<AgentTaskDto>>> GetPendingTasks()
    {
        return await Mediator.Send(new GetPendingTasksQuery());
    }

    [HttpPost("tasks/{id}/complete")]
    public async Task<ActionResult> CompleteTask(Guid id, CompleteTaskCommand command)
    {
        if (id != command.TaskId)
            return BadRequest();

        await Mediator.Send(command);
        return NoContent();
    }
}