using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.DataLayer.Tests;

public class BundlesRepositoryTests
{
    private readonly DbContextOptions<YogurtCleaningContext> _dbContextOptions;
    public BundlesRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<YogurtCleaningContext>()
            .UseInMemoryDatabase(databaseName: "TestDbForBundles")
            .Options;       
    }

    [Fact]
    public async Task AddBundle_WhenBundleeAdded_ThenBundleIdMoreThanZero()
    {
        //given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new BundlesRepository(context);
        var bundle = new Bundle
        {
            Name = "qweqwe",
            Price = 2000,
            Measure = Measure.Room,
            Services = new() { new() { Name = "qwe", Price = 1000, Unit = "Unit", IsDeleted = false } },
            IsDeleted = false
        };

        // when 
        context.Bundles.Add(bundle);
        await context.SaveChangesAsync();

        //then 
        Assert.True(bundle.Id > 0);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task DeleteSBundle_WhenCorrectIdPassed_ThenSoftDeleteApplied()
    {
        // given 
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new BundlesRepository(context);
        var bundle = new Bundle
        {
            Name = "qweqwe",
            Price = 2000,
            Measure = Measure.Room,
            Services = new() { new() { Name = "qwe", Price = 1000, Unit = "Unit", IsDeleted = false } },
            IsDeleted = false
        };

        context.Bundles.Add(bundle);
        context.SaveChanges();

        // when 
        await sut.DeleteBundle(bundle);

        //then 
        Assert.True(bundle.IsDeleted);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllBundles_WhenBundlesExist_ThenGetBundles()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new BundlesRepository(context);
        var bundle = new Bundle
        {
            Name = "qweqwe",
            Price = 2000,
            Measure = Measure.Room,
            Services = new() { new() { Name = "qwe", Price = 1000, Unit = "Unit", IsDeleted = false } },
            IsDeleted = false
        };

        context.Bundles.Add(bundle);
        context.SaveChanges();

        // when 
        var result = await sut.GetAllBundles();

        //then 
        Assert.Contains(bundle, result);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllBundles_WhenBundleIsDeleted_ThenBundleDoesNotGet()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new BundlesRepository(context);
        var bundle = new Bundle
        {
            Name = "qweqwe",
            Price = 2000,
            Measure = Measure.Room,
            Services = new() { new() { Name = "qwe", Price = 1000, Unit = "Unit", IsDeleted = false } },
            IsDeleted = true
        };

        context.Bundles.Add(bundle);
        context.SaveChanges();

        // when 
        var result = await sut.GetAllBundles();

        //then 
        Assert.DoesNotContain(bundle, result);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetListServices_WhenAllServicesInDb_ThenGetAllServices()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new BundlesRepository(context);
        var expectedservices = new List<Service>() 
        { 
            new Service() { Id = 1, Name = "full clean" , Unit = "m2", Duration = 10, Price = 20}, 
            new Service() { Id = 2, Name = "full clean2" , Unit = "m3", Duration = 5, Price = 10} 
        };
        var servicesFromRequest = new List<Service>() { new Service() { Id = 1 }, new Service() { Id = 2 } };

        context.Services.AddRange(expectedservices);
        context.SaveChanges();

        // when 
        var result = await sut.GetListServices(servicesFromRequest);

        //then 
        Assert.Equal(expectedservices.Count, result.Count);
        Assert.Equal(expectedservices[0].Name, result[0].Name);
        Assert.Equal(expectedservices[0].Unit, result[0].Unit);
        Assert.Equal(expectedservices[1].Duration, result[1].Duration);
        Assert.Equal(expectedservices[1].Price, result[1].Price);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetListServices_WhenNotAllServicesInDb_ThenGetAllServices()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new BundlesRepository(context);
        var expectedservices = new List<Service>()
        {
            new Service() { Id = 3, Name = "full clean" , Unit = "m2", Duration = 10, Price = 20}
        };
        var servicesFromRequest = new List<Service>() { new Service() { Id = 3 }, new Service() { Id = 2 } };

        context.Services.AddRange(expectedservices);
        context.SaveChanges();

        // when 
        var result = await sut.GetListServices(servicesFromRequest);

        //then 
        Assert.True(servicesFromRequest.Count > result.Count);
        context.Database.EnsureDeleted();
    }
}
