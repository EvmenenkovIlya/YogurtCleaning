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
    public void UpdateBundle_WhenUpdatePassed_ThenPropertiesValuesChandged()
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

        _bundlesRepositoryMock.Setup(b => b.GetBundle(bundle.Id)).Returns(bundle);

        // when
        _sut.UpdateBundle(updatedBundle, bundle.Id);

        // then
        Assert.Equal(bundle.Name, updatedBundle.Name);
        Assert.Equal(bundle.Price, updatedBundle.Price);
        Assert.Equal(bundle.Measure, updatedBundle.Measure);
        Assert.Equal(bundle.Services, updatedBundle.Services);
        _bundlesRepositoryMock.Verify(b => b.GetBundle(bundle.Id), Times.Once);

    }

    [Fact]
    public void GetAdditionalServices_WhenServiceIsInBundle_ThenResultDoesNotConteinIt()
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

        _servicesRepositoryMock.Setup(s => s.GetAllServices()).Returns(services);

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
        _bundlesRepositoryMock.Setup(b => b.GetBundle(bundle.Id)).Returns(bundle);

        // when
        var result = _sut.GetAdditionalServices(bundle.Id);

        // then
        Assert.DoesNotContain(service, result);
        _servicesRepositoryMock.Verify(s => s.GetAllServices(), Times.Once);
        _bundlesRepositoryMock.Verify(b => b.GetBundle(bundle.Id), Times.Once);
    }

    [Fact]
    public void GetAdditionalServices_WhenServiceIsNotInBundle_ThenResultConteinIt()
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

        _servicesRepositoryMock.Setup(s => s.GetAllServices()).Returns(services);


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

        _bundlesRepositoryMock.Setup(b => b.GetBundle(bundle.Id)).Returns(bundle);

        // when
        var result = _sut.GetAdditionalServices(bundle.Id);

        // then
        Assert.Contains(service, result);
        _servicesRepositoryMock.Verify(s => s.GetAllServices(), Times.Once);
        _bundlesRepositoryMock.Verify(b => b.GetBundle(bundle.Id), Times.Once);
    }

    [Fact]
    public void DeleteBundle_WhenValidRequestPassed_DeleteBundle()
    {
        //given
        var expectedBundle = new Bundle()
        {
            Id = 1,
            Name = "Clean all",
            Duration = 12,
            IsDeleted = false
        };

        _bundlesRepositoryMock.Setup(o => o.GetBundle(expectedBundle.Id)).Returns(expectedBundle);
        _bundlesRepositoryMock.Setup(o => o.DeleteBundle(expectedBundle));

        //when
        _sut.DeleteBundle(expectedBundle.Id);

        //then
        _bundlesRepositoryMock.Verify(c => c.DeleteBundle(expectedBundle), Times.Once);
    }

    [Fact]
    public void DeleteBundle_EmptyBundleRequest_ThrowEntityNotFoundException()
    {
        //given
        var testId = 1;
        var bundle = new Bundle();

        //when

        //then
        Assert.Throws<Exceptions.BadRequestException>(() => _sut.DeleteBundle(testId));
    }
}