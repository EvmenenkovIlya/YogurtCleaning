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
            .UseInMemoryDatabase(databaseName: "TestDb")
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
        Assert.True(result.Contains(bundle));
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
    }
}
