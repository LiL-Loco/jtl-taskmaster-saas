using Xunit;
using Moq;
using AutoMapper;
using JTLTaskMaster.Application.Common.Interfaces;
using JTLTaskMaster.Application.Features.Jobs.Queries.GetJobs;
using JTLTaskMaster.Domain.Entities;

namespace JTLTaskMaster.Application.UnitTests.Features.Jobs.Queries;

public class GetJobsQueryTests
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<ICurrentUserService> _currentUserService;
    private readonly GetJobsQueryHandler _handler;

    public GetJobsQueryTests()
    {
        _context = new Mock<IApplicationDbContext>();
        _mapper = new Mock<IMapper>();
        _currentUserService = new Mock<ICurrentUserService>();
        _handler = new GetJobsQueryHandler(_context.Object, _mapper.Object, _currentUserService.Object);

        _currentUserService.Setup(x => x.TenantId).Returns(Guid.NewGuid());
    }

    [Fact]
    public async Task Handle_ShouldReturnJobsForCurrentTenant()
    {
        // Arrange
        var tenantId = _currentUserService.Object.TenantId;
        var jobs = new List<Job>
        {
            new() { Id = Guid.NewGuid(), Name = "Job 1", TenantId = tenantId },
            new() { Id = Guid.NewGuid(), Name = "Job 2", TenantId = tenantId },
            new() { Id = Guid.NewGuid(), Name = "Other Job", TenantId = Guid.NewGuid() }
        }.AsQueryable();

        var jobDtos = new List<JobDto>
        {
            new() { Id = jobs.First().Id, Name = "Job 1" },
            new() { Id = jobs.Skip(1).First().Id, Name = "Job 2" }
        };

        _context.Setup(x => x.Jobs).Returns(MockDbSet(jobs));
        _mapper.Setup(x => x.ConfigurationProvider)
            .Returns(new MapperConfiguration(cfg => 
                cfg.CreateMap<Job, JobDto>()));

        // Act
        var result = await _handler.Handle(new GetJobsQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.DoesNotContain(result, j => j.Name == "Other Job");
    }

    private static Microsoft.EntityFrameworkCore.DbSet<T> MockDbSet<T>(IQueryable<T> data) where T : class
    {
        var mockSet = new Mock<Microsoft.EntityFrameworkCore.DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        return mockSet.Object;
    }
}