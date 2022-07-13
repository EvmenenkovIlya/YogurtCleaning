using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.DataLayer.Tests;

public class ServicesRepositoryTests
{
    private readonly DbContextOptions<YogurtCleaningContext> _dbContextOptions;
    public ServicesRepositoryTests()
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
        var actual = sut.AddService(service);

        //then 
        Assert.True(actual > 0);
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
        Assert.Contains(service, result);
    }

    [Fact]
    public void GetAllServices_WhenServiceIsDeleted_ThenServiceDoesNotGet()
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
        Assert.DoesNotContain(service, result);
    }
}