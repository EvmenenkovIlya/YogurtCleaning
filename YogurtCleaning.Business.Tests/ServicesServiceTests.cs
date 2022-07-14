using Microsoft.EntityFrameworkCore;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Tests;

public class ServicesServiceTests
{
    private readonly DbContextOptions<YogurtCleaningContext> _dbContextOptions;
    public ServicesServiceTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<YogurtCleaningContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
    }

    [Fact]
    public void UpdateService_WhenUpdatePassed_ThenPropertiesValuesChandged()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var serviceRepository = new ServicesRepository(context);
        var sut = new ServicesService(serviceRepository);

        var service = new Service
        {
            Id = 10,
            Name = "qwe",
            Price = 1000,
            Unit = "Unit",
            IsDeleted = false
        };

        var updatedService = new Service
        {
            Name = "qaz",
            Price = 9999,
            Unit = "Meter",
            IsDeleted = false
        };

        context.Add(service);
        context.SaveChanges();

        // when
        sut.UpdateService(updatedService, service.Id);

        // then
        Assert.True(service.Name == updatedService.Name);
        Assert.True(service.Price == updatedService.Price);
        Assert.True(service.Unit == updatedService.Unit);
    }
}