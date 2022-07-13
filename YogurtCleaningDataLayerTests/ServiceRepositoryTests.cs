using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaningDataLayerTests;

public class ServiceRepositoryTests
{
    private readonly DbContextOptions<YogurtCleaningContext> _dbContextOptions;
    public ServiceRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<YogurtCleaningContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
    }

    [Fact]
    public void AddService_WhenServiceAdded_ThenServiceIdMoreThanZero()
    {
        //given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new ServicesRepository(context);
        var service = new Service
        {
            Name = "qwe",
            Price = 1000,
            Unit = "Unit",
            IsDeleted = false
        };

        // when 
        context.Services.Add(service);
        context.SaveChanges();

        //then 
        Assert.True(service.Id > 0);
    }

    [Fact]
    public void DeleteService_WhenCorrectIdPassed_ThenSoftDeleteApplied()
    {
        // given 
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new ServicesRepository(context);
        var service = new Service
        {
            Name = "qwe",
            Price = 1000,
            Unit = "Unit",
            IsDeleted = false
        };

        context.Services.Add(service);
        context.SaveChanges();

        // when 
        sut.DeleteService(service.Id);

        //then 
        Assert.True(service.IsDeleted);
    }

    [Fact]
    public void GetAllComments_WhenCommentsExist_ThenGetComments()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new ServicesRepository(context);
        var service = new Service
        {
            Name = "qwe",
            Price = 1000,
            Unit = "Unit",
            IsDeleted = false
        };

        context.Services.Add(service);
        context.SaveChanges();

        // when 
        var result = sut.GetAllServices();

        //then 
        Assert.True(result.Contains(service));
    }

    [Fact]
    public void GetAllServices_WhenCommentIsDeleted_ThenCommentDoesNotGet()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new ServicesRepository(context);
        var service = new Service
        {
            Name = "qwe",
            Price = 1000,
            Unit = "Unit",
            IsDeleted = true
        };

        context.Services.Add(service);
        context.SaveChanges();

        // when 
        var result = sut.GetAllServices();

        //then 
        Assert.False(result.Contains(service));
    }
}