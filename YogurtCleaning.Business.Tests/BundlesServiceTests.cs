using Microsoft.EntityFrameworkCore;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Tests;

public class BundlesServiceTests
{
    private readonly DbContextOptions<YogurtCleaningContext> _dbContextOptions;
    public BundlesServiceTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<YogurtCleaningContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
    }

    [Fact]
    public void UpdateBundle_WhenUpdatePassed_ThenPropertiesValuesChandged()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var bundlesRepository = new BundlesRepository(context);
        var sut = new BundlesService(bundlesRepository);

        var bundle = new Bundle
        {
            Id = 10,
            Name = "qwe",
            Price = 1000,
            Measure = Measure.Room,
            IsDeleted = false
        };

        var updatedBundle = new Bundle
        {
            Name = "qaz",
            Price = 9999,
            Measure = Measure.Unit,
            IsDeleted = false
        };

        context.Add(bundle);
        context.SaveChanges();

        // when
        sut.UpdateBundle(updatedBundle, bundle.Id);

        // then
        Assert.True(bundle.Name == updatedBundle.Name);
        Assert.True(bundle.Price == updatedBundle.Price);
        Assert.True(bundle.Measure == updatedBundle.Measure);
        Assert.True(bundle.Services == updatedBundle.Services);
    }
}