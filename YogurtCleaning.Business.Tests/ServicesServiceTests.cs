using Microsoft.EntityFrameworkCore;
using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Tests;

public class ServicesServiceTests
{
    private ServicesService _sut;
    private Mock<IServicesRepository> _mockServicesRepository;

    public ServicesServiceTests()
    {
        _mockServicesRepository = new Mock<IServicesRepository>();
        _sut = new ServicesService(_mockServicesRepository.Object);
    }

    [Fact]
    public void UpdateService_WhenUpdatePassed_ThenPropertiesValuesChandged()
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
        _mockServicesRepository.Setup(s => s.GetService(service.Id)).Returns(service);

        var updatedService = new Service
        {
            Name = "qaz",
            Price = 9999,
            Unit = "Meter",
            IsDeleted = false
        };

        // when
        _sut.UpdateService(updatedService, service.Id);

        // then
        Assert.Equal(updatedService.Name, service.Name);
        Assert.Equal(updatedService.Price, service.Price);
        Assert.Equal(updatedService.Unit, service.Unit);
        _mockServicesRepository.Verify(s => s.GetService(service.Id), Times.Once);
    }
}