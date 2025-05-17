using Xunit;
using Moq;
using JTLTaskMaster.Api.Controllers;
using JTLTaskMaster.Application.Features.Jobs.Commands.CreateJob;
using JTLTaskMaster.Application.Features.Jobs.Queries.GetJobs;

namespace JTLTaskMaster.Api.UnitTests.Controllers;

public class JobsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly JobsController _controller;

    public JobsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new JobsController();
        typeof(BaseApiController)
            .GetProperty(nameof(BaseApiController.Mediator))!
            .SetValue(_controller, _mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnJobs()
    {
        // Arrange
        var jobs = new List<JobDto>
        {
            new() { Id = Guid.NewGuid(), Name = "Test Job 1" },
            new() { Id = Guid.NewGuid(), Name = "Test Job 2" }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetJobsQuery>(), default))
            .ReturnsAsync(jobs);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<JobDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task Create_ShouldReturnNewId()
    {
        // Arrange
        var command = new CreateJobCommand
        {
            Name = "Test Job",
            Description = "Test Description",
            IsEnabled = true
        };

        var newId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(command, default))
            .ReturnsAsync(newId);

        // Act
        var result = await _controller.Create(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Guid>(okResult.Value);
        Assert.Equal(newId, returnValue);
    }
}