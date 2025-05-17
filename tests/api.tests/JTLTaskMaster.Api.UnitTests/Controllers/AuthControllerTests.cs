using Xunit;
using Moq;
using JTLTaskMaster.Api.Controllers;
using JTLTaskMaster.Application.Features.Auth.Commands.Register;
using JTLTaskMaster.Application.Features.Auth.Queries;

namespace JTLTaskMaster.Api.UnitTests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AuthController();
        // Inject mediator using reflection
        typeof(BaseApiController)
            .GetProperty(nameof(BaseApiController.Mediator))!
            .SetValue(_controller, _mediatorMock.Object);
    }

    [Fact]
    public async Task Register_ShouldReturnSuccess_WhenValidCommand()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Email = "test@test.com",
            Password = "Test123!",
            FirstName = "Test",
            LastName = "User"
        };

        _mediatorMock
            .Setup(m => m.Send(command, default))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _controller.Register(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Result>(okResult.Value);
        Assert.True(returnValue.Succeeded);
    }

    [Fact]
    public async Task Login_ShouldReturnToken_WhenValidCredentials()
    {
        // Arrange
        var query = new LoginQuery
        {
            Email = "test@test.com",
            Password = "Test123!"
        };

        var response = new LoginResponse
        {
            Token = "test-token",
            RefreshToken = "refresh-token",
            Expiration = DateTime.UtcNow.AddDays(1)
        };

        _mediatorMock
            .Setup(m => m.Send(query, default))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.Login(query);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<LoginResponse>(okResult.Value);
        Assert.Equal(response.Token, returnValue.Token);
    }
}