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
    private Mock<ILogger<ServicesController>> _mockLogger;
    private Mock<IServicesService> _mockServicesService;
    private Mock<IServicesRepository> _mockServicesRepository;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<ServicesController>>();
        _mockServicesRepository = new Mock<IServicesRepository>();
        _mockServicesService = new Mock<IServicesService>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigStorage>()));
        _sut = new ServicesController(_mockLogger.Object, _mockServicesRepository.Object, _mockServicesService.Object, _mapper);
    }

    [Test]
    public void AddServices_WhenValidRequestPassed_ThenCreatedResultRecived()
    {
        // given
        _mockServicesService.Setup(o => o.AddService(It.IsAny<Service>())).Returns(1);
        var service = new ServiceRequest()
        {
            Name = "Service name",
            Price = 100500,
            Unit = "Hour"
        };

        // when
        var actual = _sut.AddService(service);

        // then
        var actualResult = actual.Result as CreatedResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.True((int)actualResult.Value == 1);
        _mockServicesService.Verify(o => o.AddService(
            It.Is<Service>
            (s => s.Name == service.Name &&
            s.Price == service.Price &&
            s.Unit == service.Unit)), Times.Once);
        
    }

    [Test]
    public void GetService_WhenCorrectIdPassed_ThenOkRecieved()
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
        _mockServicesService.Setup(o => o.GetService(service.Id)).Returns(service);

        // when
        var actual = _sut.GetService(service.Id);

        // then
        var actualResult = actual.Result as ObjectResult;
        var serviceResponse = actualResult.Value as ServiceResponse;

            Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
            Assert.That(expectedId, Is.EqualTo((int)actualResult.Value));
            _mockServicesService.Verify(o => o.AddService(
                It.Is<Service>
                (s => s.Name == service.Name &&
                s.Price == service.Price &&
                s.Unit == service.Unit)), Times.Once);
            
        }

        _mockServicesService.Verify(o => o.GetService(service.Id), Times.Once);

    }

    [Test]
    public void GetAllServices_WhenCorrectRequestPassed_ThenOkRecieved()
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
                Orders = new List<Order>()
            };
            _mockServicesService.Setup(o => o.GetService(service.Id)).Returns(service);

            // when
            var actual = _sut.GetService(service.Id);

            // then
            var actualResult = actual.Result as ObjectResult;
            var serviceResponse = actualResult.Value as ServiceResponse;

            Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(service.Id, Is.EqualTo(serviceResponse.Id));
            Assert.That(service.Name, Is.EqualTo(serviceResponse.Name));
            Assert.That(service.Price, Is.EqualTo(serviceResponse.Price));
            Assert.That(service.Unit, Is.EqualTo(serviceResponse.Unit));
            Assert.That(service.Duration, Is.EqualTo(serviceResponse.Duration));

            _mockServicesService.Verify(o => o.GetService(service.Id), Times.Once);
        }

        [Test]
        public void GetAllServices_WhenCorrectRequestPassed_ThenOkRecieved()
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
                    Orders = new List<Order>()
                },
                new Service()
                {
                    Id = 2,
                    Name = "Service name2",
                    Price = 100502,
                    Unit = "Hour",
                    Duration = 2,
                    IsDeleted = false,
                    Orders = new List<Order>()
                },
                new Service()
                {
                    Id = 2,
                    Name = "Service name3",
                    Price = 100503,
                    Unit = "Hour",
                    Duration = 3,
                    IsDeleted = false,
                    Orders = new List<Order>()
                }
            };
            _mockServicesRepository.Setup(o => o.GetAllServices()).Returns(expectedService);

            // when
            var actual = _sut.GetAllServices();

            // then
            var actualResult = actual.Result as ObjectResult;
            var servicesResponse = actualResult.Value as List<ServiceResponse>;

            Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(expectedService.Count, Is.EqualTo(servicesResponse.Count));
            Assert.That(expectedService[0].Id, Is.EqualTo(servicesResponse[0].Id));
            Assert.That(expectedService[1].Name, Is.EqualTo(servicesResponse[1].Name));
            Assert.That(expectedService[2].Price, Is.EqualTo(servicesResponse[2].Price));
            Assert.That(expectedService[0].Unit, Is.EqualTo(servicesResponse[0].Unit));
            Assert.That(expectedService[1].Duration, Is.EqualTo(servicesResponse[1].Duration));           
        }

        [Test]
        public void UpdateService_WhenCorrectRequestPassed_ThenNoContentRecieved()
        {
            // given
            var service = new Service()
            {
                Id = 2,
                Name = "Service name3",
                Price = 100503,
                Unit = "Hour",
                Duration = 3,
                IsDeleted = false,
                Orders = new List<Order>()
            }
        };
        _mockServicesRepository.Setup(o => o.GetAllServices()).Returns(expectedService);

        // when
        var actual = _sut.GetAllServices();

        // then
        var actualResult = actual.Result as ObjectResult;
        var servicesResponse = actualResult.Value as List<ServiceResponse>;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.True(servicesResponse.Count == expectedService.Count);
        Assert.True(servicesResponse[0].Id == expectedService[0].Id);
        Assert.True(servicesResponse[1].Name == expectedService[1].Name);
        Assert.True(servicesResponse[2].Price == expectedService[2].Price);
        Assert.True(servicesResponse[0].Unit == expectedService[0].Unit);
        Assert.True(servicesResponse[1].Duration == expectedService[1].Duration);
       
    }

    [Test]
    public void UpdateService_WhenCorrectRequestPassed_ThenNoContentRecieved()
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
        var actual = _sut.UpdateService(newProperty, service.Id);

        // then
        var actualResult = actual as NoContentResult;
        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        _mockServicesService.Verify(o => o.UpdateService(
            It.Is<Service>
            (s =>s.Name == newProperty.Name &&
            s.Price == newProperty.Price &&
            s.Unit == newProperty.Unit),
           It.Is<int>(i => i == service.Id)), Times.Once);
    }

    [Test]
    public void DeleteService_WhenCorrectRequestPassed_ThenOkRecieved()
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
        var actual = _sut.DeleteService(service.Id);

        // then
        var actualResult = actual as OkResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        _mockServicesService.Verify(o => o.DeleteService(service.Id, It.IsAny<UserValues>()), Times.Once);
    }
}
