# Testing Dokumentation

## Unit Tests
public class JobServiceTests
{
    [Fact]
    public async Task CreateJob_ShouldSucceed()
    {
        // Arrange
        var service = new JobService();
        
        // Act
        var result = await service.CreateJob(new Job());
        
        // Assert
        Assert.NotNull(result);
    }
}

## Integration Tests
public class JobControllerTests
{
    [Fact]
    public async Task GetJobs_ShouldReturnJobs()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/api/jobs");
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
}

## E2E Tests
describe('Job Creation', () => {
    it('should create a new job', () => {
        cy.visit('/jobs/new');
        cy.get('[data-testid="job-name"]').type('Test Job');
        cy.get('[data-testid="save-button"]').click();
        cy.url().should('include', '/jobs');
    });
});
