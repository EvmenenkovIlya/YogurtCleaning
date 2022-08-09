using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.Controllers;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.Models;
using YogurtCleaning.API;
using YogurtCleaning.Business;

namespace YogurtCleaning.Tests;

public class ServicesControllerTests
{
    private ServicesController _sut;

    private Mock<IServicesService> _mockServicesService;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _mockServicesService = new Mock<IServicesService>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigStorage>()));
        _sut = new ServicesController( _mockServicesService.Object, _mapper);
    }

    [Test]
    public async Task AddServices_WhenValidRequestPassed_ThenCreatedResultRecived()
    {
        // given
        int expectedId = 1;
        _mockServicesService.Setup(o => o.AddService(It.IsAny<Service>())).ReturnsAsync(expectedId);
        var service = new ServiceRequest()
        {
            Name = "Service name",
            Price = 100500,
            Unit = "Hour"
        };

        // when
        var actual = await _sut.AddService(service);

        // then
        var actualResult = actual.Result as CreatedResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That((int)actualResult.Value, Is.EqualTo(expectedId));
        _mockServicesService.Verify(o => o.AddService(
            It.Is<Service>
            (s => s.Name == service.Name &&
            s.Price == service.Price &&
            s.Unit == service.Unit)), Times.Once);

    }

    [Test]
    public async Task GetService_WhenCorrectIdPassed_ThenOkRecieved()
    {
        // given

        var service = new Service()
        {
            Id = 2,
            Name = "Service name",
            Price = 100500,
            Unit = "Hour",
            Duration = 1,
            IsDeleted = false,
            Orders = new List<Order>()
        };
        _mockServicesService.Setup(o => o.GetService(service.Id)).ReturnsAsync(service);

        // when
        var  actual = await _sut.GetService(service.Id);

        // then
        var actualResult = actual.Result as ObjectResult;
        var serviceResponse = actualResult.Value as ServiceResponse;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.Multiple(() =>
        {
            Assert.That(serviceResponse.Id, Is.EqualTo(service.Id));
            Assert.That(serviceResponse.Name, Is.EqualTo(service.Name));
            Assert.That(serviceResponse.Price, Is.EqualTo(service.Price));
            Assert.That(serviceResponse.Unit, Is.EqualTo(service.Unit));
            Assert.That(serviceResponse.Duration, Is.EqualTo(service.Duration));
        });
        _mockServicesService.Verify(o => o.GetService(service.Id), Times.Once);
    }

    [Test]
    public async Task GetAllServices_WhenCorrectRequestPassed_ThenOkRecieved()
    {
        // given
        var expectedService = new List<Service>()
        {
            new Service()
            {
                Id = 2,
                Name = "Service name1",
                Price = 100501,
                Unit = "Hour",
                Duration = 1,
                IsDeleted = false,
            },
            new Service()
            {
                Id = 2,
                Name = "Service name2",
                Price = 100502,
                Unit = "Hour",
                Duration = 2,
                IsDeleted = false,
            },
            new Service()
            {
                Id = 2,
                Name = "Service name3",
                Price = 100503,
                Unit = "Hour",
                Duration = 3,
                IsDeleted = false,
            }
        };
        _mockServicesService.Setup(o => o.GetAllServices()).ReturnsAsync(expectedService).Verifiable();

        // when
        var actual = await _sut.GetAllServices();

        // then
        var actualResult = actual.Result as ObjectResult;
        var servicesResponse = actualResult.Value as List<ServiceResponse>;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(servicesResponse.Count, Is.EqualTo(expectedService.Count));
        Assert.Multiple(() =>
        {
            Assert.That(servicesResponse[0].Id, Is.EqualTo(expectedService[0].Id));
            Assert.That(servicesResponse[1].Name, Is.EqualTo(expectedService[1].Name));
            Assert.That(servicesResponse[2].Price, Is.EqualTo(expectedService[2].Price));
            Assert.That(servicesResponse[0].Unit, Is.EqualTo(expectedService[0].Unit));
            Assert.That(servicesResponse[1].Duration, Is.EqualTo(expectedService[1].Duration));
        });
    }

    [Test]
    public async Task UpdateService_WhenCorrectRequestPassed_ThenNoContentRecieved()
    {
        // given
        var service = new Service()
        {
            Id = 2,
            Name = "Service name",
            Price = 100500,
            Unit = "Hour",
            IsDeleted = false,
            Orders = new List<Order>()
        };

        var newProperty = new ServiceRequest()
        {
            Name = "Service name",
            Price = 9999,
            Unit = "Hour"
        };

        _mockServicesService.Setup(o => o.UpdateService(service, service.Id));

        // when
        var actual = await _sut.UpdateService(newProperty, service.Id);

        // then
        var actualResult = actual as NoContentResult;
        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        _mockServicesService.Verify(o => o.UpdateService(
            It.Is<Service>
            (s => s.Name == newProperty.Name &&
            s.Price == newProperty.Price &&
            s.Unit == newProperty.Unit),
           It.Is<int>(i => i == service.Id)), Times.Once);
    }

    [Test]
    public async Task DeleteService_WhenCorrectRequestPassed_ThenOkRecieved()
    {
        // given
        var service = new Service()
        {
            Id = 2,
            Name = "Service name",
            Price = 100500,
            Unit = "Hour",
            IsDeleted = false,
            Orders = new List<Order>()
        };

        // when
        var actual = await _sut.DeleteService(service.Id);

        // then
        var actualResult = actual as NoContentResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        _mockServicesService.Verify(o => o.DeleteService(service.Id, It.IsAny<UserValues>()), Times.Once);
    }
}