using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.DataLayer;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaningDataLayerTests;

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
    public void AddBundle_WhenBundleeAdded_ThenBundleIdMoreThanZero()
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
        context.SaveChanges();

        //then 
        Assert.True(bundle.Id > 0);
    }

    [Fact]
    public void DeleteSBundle_WhenCorrectIdPassed_ThenSoftDeleteApplied()
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
        sut.DeleteBundle(bundle.Id);

        //then 
        Assert.True(bundle.IsDeleted);
    }

    [Fact]
    public void GetAllBundles_WhenBundlesExist_ThenGetBundles()
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
        var result = sut.GetAllBundles();

        //then 
        Assert.True(result.Contains(bundle));
    }

    [Fact]
    public void GetAllBundles_WhenBundleIsDeleted_ThenBundleDoesNotGet()
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
        var result = sut.GetAllBundles();

        //then 
        Assert.False(result.Contains(bundle));
    }
}
