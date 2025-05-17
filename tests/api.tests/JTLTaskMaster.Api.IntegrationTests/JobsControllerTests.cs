using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using JTLTaskMaster.Api;
using JTLTaskMaster.Application.Features.Jobs.Queries.GetJobs;

namespace JTLTaskMaster.Api.IntegrationTests;

public class JobsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public JobsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetJobs_ReturnsSuccessStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/jobs");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreateJob_ReturnsCreatedResponse()
    {
        // Arrange
        var client = _factory.CreateClient();
        var job = new CreateJobCommand
        {
            Name = "Test Job",
            Description = "Test Description",
            IsEnabled = true
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/jobs", job);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }
}