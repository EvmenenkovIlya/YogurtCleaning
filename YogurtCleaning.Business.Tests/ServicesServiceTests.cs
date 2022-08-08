using Microsoft.EntityFrameworkCore;
using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Tests;

public class ServicesServiceTests
{
    private ServicesService _sut;
    private Mock<IServicesRepository> _mockServicesRepository;
    private UserValues userValue;

    public ServicesServiceTests()
    {
        _mockServicesRepository = new Mock<IServicesRepository>();
        _sut = new ServicesService(_mockServicesRepository.Object);
    }

    [Fact]
    public async Task UpdateService_WhenUpdatePassed_ThenPropertiesValuesChandged()
    {
        // given
        var service = new Service
        {
            Id = 10,
            Name = "qwe",
            Price = 1000,
            Unit = "Unit",
            IsDeleted = false
        };
        _mockServicesRepository.Setup(s => s.GetService(service.Id)).ReturnsAsync(service);

        var updatedService = new Service
        {
            Name = "qaz",
            Price = 9999,
            Unit = "Meter",
            IsDeleted = false
        };

        // when
        await _sut.UpdateService(updatedService, service.Id);

        // then
        Assert.Equal(updatedService.Name, service.Name);
        Assert.Equal(updatedService.Price, service.Price);
        Assert.Equal(updatedService.Unit, service.Unit);
        _mockServicesRepository.Verify(s => s.GetService(service.Id), Times.Once);
    }

    [Fact]
    public async Task GetAllService_WhenValidRequestPassed_OrdersReceived()
    {
        //given
        var services = new List<Service>
        {
            new Service()
            {
                Id = 4,
                Name = "Service name1",
                Price = 100500,
                Unit = "Hour",
                Duration = 1,
                IsDeleted = false,
                Orders = new List<Order>()

            },
            new Service()
            {
                Id = 2,
                Name = "Service name2",
                Price = 10500,
                Unit = "Hour",
                Duration = 1,
                IsDeleted = false,
                Orders = new List<Order>()

            }
        };
        _mockServicesRepository.Setup(o => o.GetAllServices()).ReturnsAsync(services).Verifiable();

        //when
        var actual = await _sut.GetAllServices();

        //then
        Assert.NotNull(actual);
        Assert.Equal(services.Count, actual.Count);
        _mockServicesRepository.Verify(c => c.GetAllServices(), Times.Once);
    }

    [Fact]
    public async Task DeleteService_WhenValidRequestPassed_DeleteCleaner()
    {
        //given        
        var expectedService = new Service()
        {
            Id = 2,
            Name = "Service name2",
            Price = 10500,
            Unit = "Hour",
            Duration = 1,
            IsDeleted = false,
            Orders = new List<Order>()

        };

        _mockServicesRepository.Setup(o => o.GetService(expectedService.Id)).ReturnsAsync(expectedService);
        _mockServicesRepository.Setup(o => o.DeleteService(expectedService));
        userValue = new UserValues() { Email = "AdamSmith@gmail.com", Role = Role.Cleaner, Id = 1 };

        //when
        await _sut.DeleteService(expectedService.Id, userValue);

        //then
        _mockServicesRepository.Verify(c => c.DeleteService(expectedService), Times.Once);
        _mockServicesRepository.Verify(c => c.GetService(It.IsAny<int>()), Times.Once);
    }
}