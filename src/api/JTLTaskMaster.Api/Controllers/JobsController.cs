using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JTLTaskMaster.Application.Features.Jobs.Commands;
using JTLTaskMaster.Application.Features.Jobs.Queries;

namespace JTLTaskMaster.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<JobDto>>> GetAll()
    {
        return await _mediator.Send(new GetJobsQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobDto>> Get(Guid id)
    {
        var job = await _mediator.Send(new GetJobByIdQuery { Id = id });
        if (job == null) return NotFound();
        return job;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateJobCommand command)
    {
        var jobId = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = jobId }, jobId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateJobCommand command)
    {
        if (id != command.Id) return BadRequest();
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteJobCommand { Id = id });
        return NoContent();
    }

    [HttpPost("{id}/start")]
    public async Task<IActionResult> Start(Guid id)
    {
        await _mediator.Send(new StartJobCommand { Id = id });
        return NoContent();
    }
}