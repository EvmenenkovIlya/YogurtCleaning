using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;
using Moq;

namespace YogurtCleaning.Business.Tests;

public class BundlesServiceTests
{
    private BundlesService _sut;
    private Mock<IBundlesRepository> _bundlesRepositoryMock;
    private Mock<IServicesRepository> _servicesRepositoryMock;

    public BundlesServiceTests()
    {
        _bundlesRepositoryMock = new Mock<IBundlesRepository>();
        _servicesRepositoryMock = new Mock<IServicesRepository>();
        _sut = new BundlesService(_bundlesRepositoryMock.Object, _servicesRepositoryMock.Object);
    }

    [Fact]
    public async Task GetBundle_WhenBundleInDb_BundleReceived()
    {
        //given        
        var bundleInDb = new Bundle()
        {
            Id = 1,
            Name = "qwe",
            Price = 1000,
            Measure = Measure.Room,
            IsDeleted = false
        };
        
        _bundlesRepositoryMock.Setup(o => o.GetBundle(bundleInDb.Id)).ReturnsAsync(bundleInDb);

        //when
        var actual = await _sut.GetBundle(bundleInDb.Id);

        //then
        _bundlesRepositoryMock.Verify(c => c.GetBundle(bundleInDb.Id), Times.Once);
    }

    [Fact]
    public async Task GetBundle_WhenBundleNotInDb_ThrowBadRequestException()
    {
        //given        
        int idNotFromDb = 5;

        //when
        //then
        await Assert.ThrowsAsync<Exceptions.BadRequestException>(() => _sut.GetBundle(idNotFromDb));
    }

    [Fact]
    public async Task UpdateBundle_WhenUpdatePassed_ThenPropertiesValuesChandged()
    {
        // given
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

        _bundlesRepositoryMock.Setup(b => b.GetBundle(bundle.Id)).ReturnsAsync(bundle);

        // when
        await _sut.UpdateBundle(updatedBundle, bundle.Id);

        // then
        Assert.Equal(updatedBundle.Name, bundle.Name);
        Assert.Equal(updatedBundle.Price, bundle.Price);
        Assert.Equal(updatedBundle.Measure, bundle.Measure);
        Assert.Equal(updatedBundle.Services, bundle.Services);
        _bundlesRepositoryMock.Verify(b => b.GetBundle(bundle.Id), Times.Once);
    }

    [Fact]
    public async Task GetAdditionalServices_WhenServiceIsInBundle_ThenResultDoesNotConteinIt()
    {
        // given
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

        _servicesRepositoryMock.Setup(s => s.GetAllServices()).ReturnsAsync(services).Verifiable();

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
        _bundlesRepositoryMock.Setup(b => b.GetBundle(bundle.Id)).ReturnsAsync(bundle);

        // when
        var result = await _sut.GetAdditionalServices(bundle.Id);

        // then
        Assert.DoesNotContain(service, result);
        _servicesRepositoryMock.Verify(s => s.GetAllServices(), Times.Once);
        _bundlesRepositoryMock.Verify(b => b.GetBundle(bundle.Id), Times.Once);
    }

    [Fact]
    public async Task GetAdditionalServices_WhenServiceIsNotInBundle_ThenResultConteinIt()
    {
        // given
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

        _servicesRepositoryMock.Setup(s => s.GetAllServices()).ReturnsAsync(services).Verifiable(); ;


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

        _bundlesRepositoryMock.Setup(b => b.GetBundle(bundle.Id)).ReturnsAsync(bundle);

        // when
        var result = await _sut.GetAdditionalServices(bundle.Id);

        // then
        Assert.Contains(service, result);
        _servicesRepositoryMock.Verify(s => s.GetAllServices(), Times.Once);
        _bundlesRepositoryMock.Verify(b => b.GetBundle(bundle.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteBundle_WhenValidRequestPassed_DeleteBundle()
    {
        //given
        var expectedBundle = new Bundle()
        {
            Id = 1,
            Name = "Clean all",
            Duration = 12,
            IsDeleted = false
        };

        _bundlesRepositoryMock.Setup(o => o.GetBundle(expectedBundle.Id)).ReturnsAsync(expectedBundle);
        _bundlesRepositoryMock.Setup(o => o.DeleteBundle(expectedBundle));

        //when
        await _sut.DeleteBundle(expectedBundle.Id);

        //then
        _bundlesRepositoryMock.Verify(c => c.DeleteBundle(expectedBundle), Times.Once);
    }

    [Fact]
    public async Task DeleteBundle_EmptyBundleRequest_ThrowEntityNotFoundException()
    {
        //given
        var testId = 1;
        var bundle = new Bundle();

        //when

        //then
        await Assert.ThrowsAsync<Exceptions.BadRequestException>(() => _sut.DeleteBundle(testId));
    }
    [Fact]
    public async Task CreateBundle_WhenValidRequestPassed_BundleAdded()
    {
        //given        
        _bundlesRepositoryMock.Setup(c => c.AddBundle(It.IsAny<Bundle>()))
             .ReturnsAsync(1);
        var expectedId = 1;
        var expectedServices = new List<Service>() { new Service() { Id = 1 }, new Service() { Id = 2 } };
        var bundle = new Bundle()
        {
            Name = "Clean all",
            Duration = 12,
            Services = new List<Service>() { new Service() { Id = 1 }, new Service() { Id = 2 } },
            IsDeleted = false
        };
        _bundlesRepositoryMock.Setup(c => c.GetServices(bundle.Services)).ReturnsAsync(expectedServices);

        //when
        var actual = await _sut.AddBundle(bundle);

        //then
        Assert.Equal(expectedId, actual);
        _bundlesRepositoryMock.Verify(c => c.AddBundle(bundle), Times.Once);
    }

    [Fact]
    public async Task CreateBundle_WhenValidRequestPassed_BundleAdded_ThrowBadRequestException()
    {
        //given        
        var expectedServices = new List<Service>() { new Service() { Id = 1 }};
        var bundle = new Bundle()
        {
            Name = "Clean all",
            Duration = 12,
            Services = new List<Service>() { new Service() { Id = 1 }, new Service() { Id = 2 } },
            IsDeleted = false
        };
        _bundlesRepositoryMock.Setup(c => c.GetServices(bundle.Services)).ReturnsAsync(expectedServices);

        //when

        //then
        await Assert.ThrowsAsync<Exceptions.BadRequestException>(() => _sut.AddBundle(bundle));
    }
}