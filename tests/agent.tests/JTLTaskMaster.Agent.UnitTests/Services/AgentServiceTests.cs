using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using JTLTaskMaster.Agent.Services;
using JTLTaskMaster.Agent.Config;

namespace JTLTaskMaster.Agent.UnitTests.Services;

public class AgentServiceTests
{
    private readonly Mock<ILogger<AgentService>> _logger;
    private readonly Mock<IOptions<AgentConfig>> _config;
    private readonly Mock<ITaskExecutorFactory> _taskExecutorFactory;

    public AgentServiceTests()
    {
        _logger = new Mock<ILogger<AgentService>>();
        _config = new Mock<IOptions<AgentConfig>>();
        _taskExecutorFactory = new Mock<ITaskExecutorFactory>();

        _config.Setup(x => x.Value).Returns(new AgentConfig
        {
            AgentId = "test-agent",
            HubUrl = "http://localhost:5000/hubs/agent"
        });
    }

    [Fact]
    public async Task StartAsync_ShouldConnectToHub()
    {
        // Arrange
        var service = new AgentService(
            _logger.Object,
            _config.Object,
            _taskExecutorFactory.Object);

        // Act
        await service.StartAsync(CancellationToken.None);

        // Assert
        // Verify hub connection was attempted
        _logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Connected to hub")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}