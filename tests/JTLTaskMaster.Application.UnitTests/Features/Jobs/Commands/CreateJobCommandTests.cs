using Xunit;
using Moq;
using JTLTaskMaster.Application.Common.Interfaces;
using JTLTaskMaster.Application.Features.Jobs.Commands.CreateJob;
using JTLTaskMaster.Domain.Entities;
using JTLTaskMaster.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JTLTaskMaster.Application.UnitTests.Features.Jobs.Commands;

public class CreateJobCommandTests
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly Mock<ICurrentUserService> _currentUserService;
    private readonly CreateJobCommandHandler _handler;

    public CreateJobCommandTests()
    {
        _context = new Mock<IApplicationDbContext>();
        _currentUserService = new Mock<ICurrentUserService>();
        _handler = new CreateJobCommandHandler(_context.Object, _currentUserService.Object);

        _currentUserService.Setup(x => x.TenantId).Returns(Guid.NewGuid());
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateJobWithTasks()
    {
        // Arrange
        var command = new CreateJobCommand
        {
            Name = "Test Job",
            Description = "Test Description",
            IsEnabled = true,
            Tasks = new List<CreateJobTaskDto>
            {
                new() { Type = "test", Parameters = "{}", Order = 0, IsEnabled = true }
            }
        };

        Job? savedJob = null;
        _context.Setup(x => x.Jobs.AddAsync(It.IsAny<Job>(), It.IsAny<CancellationToken>()))
            .Callback<Job, CancellationToken>((job, _) => savedJob = job);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        Assert.NotNull(savedJob);
        Assert.Equal(command.Name, savedJob!.Name);
        Assert.Equal(command.Description, savedJob.Description);
        Assert.Equal(command.IsEnabled, savedJob.IsEnabled);
        Assert.Single(savedJob.Tasks);
        Assert.Equal(JobStatus.Pending, savedJob.Status);
        Assert.Equal(_currentUserService.Object.TenantId, savedJob.TenantId);
    }
}