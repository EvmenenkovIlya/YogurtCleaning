using Microsoft.EntityFrameworkCore;
using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Tests;

public class BundlesServiceTests
{
    private BundlesService _sut;
    private Mock<IBundlesRepository> _mockBundlesRepository;
    private Mock<IServicesRepository> _mockServicesRepository;

    private void Setup()
    {
        _mockBundlesRepository = new Mock<IBundlesRepository>();
        _mockServicesRepository = new Mock<IServicesRepository>();
        _sut = new BundlesService(_mockBundlesRepository.Object, _mockServicesRepository.Object);
    }

    [Fact]
    public void GetAdditionalServices_WhenServiceIsInBundle_ThenResultDoesNotConteinIt()
    {
        // given
        Setup();
        var services = new List<Service>();
        var service = new Service()
        {
            Id = 11,
            Name = "qwe",
            Price = 1000,
            Unit = "Unit",
            Duration = 1,
            IsDeleted = false
        };

        services.Add(service);

        _mockServicesRepository.Setup(s => s.GetAllServices()).Returns(services);

        var bundle = new Bundle
        {
            Id = 7,
            Name = "zzz",
            Services = new List<Service> { new()
                {
                    Id = 11, 
                    Name = "qwe", 
                    Price = 1000,
                    Unit = "Unit",
                    Duration = 1,
                    IsDeleted = false
                } 
            }
        };
        _mockBundlesRepository.Setup(b => b.GetBundle(bundle.Id)).Returns(bundle);

        // when
        var result = _sut.GetAdditionalServices(bundle.Id);

        // then
        Assert.DoesNotContain(service, result);
        _mockServicesRepository.Verify(s => s.GetAllServices(), Times.Once);
        _mockBundlesRepository.Verify(b => b.GetBundle(bundle.Id), Times.Once);
    }

    [Fact]
    public void GetAdditionalServices_WhenServiceIsNotInBundle_ThenResultConteinIt()
    {
        // given
        Setup();

        var services = new List<Service>();
        var service = new Service()
        {
            Id = 5,
            Name = "qwe",
            Price = 1000,
            Unit = "Unit",
            Duration = 1,
            IsDeleted = false
        };

        services.Add(service);

        _mockServicesRepository.Setup(s => s.GetAllServices()).Returns(services);


        var bundle = new Bundle
        {
            Id = 7,
            Name = "zzz",
            Services = new List<Service> { new()
                {
                    Id = 100, Name = "qwa",
                    Price = 1000,
                    Unit = "Unit",
                    Duration = 1,
                    IsDeleted = false
                }
            }
        };

        _mockBundlesRepository.Setup(b => b.GetBundle(bundle.Id)).Returns(bundle);

        // when
        var result = _sut.GetAdditionalServices(bundle.Id);

        // then
        Assert.Contains(service, result);
        _mockServicesRepository.Verify(s => s.GetAllServices(), Times.Once);
        _mockBundlesRepository.Verify(b => b.GetBundle(bundle.Id), Times.Once);
    }
}