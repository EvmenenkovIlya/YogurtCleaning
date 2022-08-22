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
    public async Task AddService_WhenServiceAdded_ThenServiceIdMoreThanZero()
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
        var actual = await sut.AddService(service);

        //then 
        Assert.True(actual > 0);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task DeleteService_WhenCorrectIdPassed_ThenSoftDeleteApplied()
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
        await sut.DeleteService(service);

        //then 
        Assert.True(service.IsDeleted);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllComments_WhenCommentsExist_ThenGetComments()
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
        var result = await sut.GetAllServices();

        //then 
        Assert.Contains(service, result);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllServices_WhenServiceIsDeleted_ThenServiceDoesNotGet()
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
        var result = await sut.GetAllServices();

        //then 
        Assert.DoesNotContain(service, result);
        context.Database.EnsureDeleted();
    }
}