using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JTLTaskMaster.Api.Controllers.Base;

namespace JTLTaskMaster.Api.Controllers;

[Authorize]
public class TasksController : BaseApiController
{
    [HttpGet("types")]
    public async Task<ActionResult<List<TaskTypeDto>>> GetTaskTypes()
    {
        return await Mediator.Send(new GetTaskTypesQuery());
    }

    [HttpGet("{id}/status")]
    public async Task<ActionResult<TaskStatusDto>> GetTaskStatus(Guid id)
    {
        return await Mediator.Send(new GetTaskStatusQuery { Id = id });
    }

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult> CancelTask(Guid id)
    {
        await Mediator.Send(new CancelTaskCommand { Id = id });
        return NoContent();
    }
}