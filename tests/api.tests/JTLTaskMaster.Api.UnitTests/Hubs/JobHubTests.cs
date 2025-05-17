using Microsoft.AspNetCore.SignalR;
using Xunit;
using Moq;
using JTLTaskMaster.Api.Hubs;
using JTLTaskMaster.Application.Common.Interfaces;

namespace JTLTaskMaster.Api.UnitTests.Hubs;

public class JobHubTests
{
    private readonly Mock<IHubCallerClients> _mockClients;
    private readonly Mock<ICurrentUserService> _currentUserService;
    private readonly Mock<HubCallerContext> _mockContext;
    private readonly JobHub _hub;

    public JobHubTests()
    {
        _mockClients = new Mock<IHubCallerClients>();
        _currentUserService = new Mock<ICurrentUserService>();
        _mockContext = new Mock<HubCallerContext>();

        _hub = new JobHub(_currentUserService.Object)
        {
            Clients = _mockClients.Object,
            Context = _mockContext.Object
        };
    }

    [Fact]
    public async Task OnConnectedAsync_ShouldAddToTenantGroup()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var connectionId = "test-connection";
        
        _currentUserService.Setup(x => x.TenantId).Returns(tenantId);
        _mockContext.Setup(x => x.ConnectionId).Returns(connectionId);

        // Act
        await _hub.OnConnectedAsync();

        // Assert
        _mockClients.Verify(
            x => x.Group(tenantId.ToString()).SendAsync(
                "UserConnected",
                connectionId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}