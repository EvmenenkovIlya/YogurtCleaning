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


namespace YogurtCleaning.Tests
{
    public class ServicesControllerTests
    {
        private ServicesController _sut;
        private Mock<ILogger<ServicesController>> _mockLogger;
        private Mock<IServicesService> _mockServicesService;
        private Mock<IServicesRepository> _mockServicesRepository;
        private Mock<IMapper> _mockMapper;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<ServicesController>>();
            _mockServicesRepository = new Mock<IServicesRepository>();
            _mockServicesService = new Mock<IServicesService>();
            _mockMapper = new Mock<IMapper>();
            _sut = new ServicesController(_mockLogger.Object, _mockServicesRepository.Object, _mockServicesService.Object, _mockMapper.Object);
        }

        [Test]
        public void AddServices_WhenValidRequestPassed_ThenCreatedResultRecived()
        {
            // given
            _mockServicesRepository.Setup(o => o.AddService(It.IsAny<Service>())).Returns(1);
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
            _mockServicesRepository.Verify(o => o.AddService(It.IsAny<Service>()), Times.Once);
        }

        [Test]
        public void GetService_WhenCorrectIdPassed_ThenOkRecieved()
        {
            // given
            var expectedService = new Service()
            {
                Id = 2,
                Name = "Service name",
                Price = 100500,
                Unit = "Hour",
                IsDeleted = false,
                Orders = new List<Order>()
            };
            _mockServicesRepository.Setup(o => o.GetService(expectedService.Id)).Returns(expectedService);

            // when
            var actual = _sut.GetService(expectedService.Id);

            // then
            var actualResult = actual.Result as ObjectResult;

            Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            _mockServicesRepository.Verify(o => o.GetService(expectedService.Id), Times.Once);

        }

        [Test]
        public void GetAllServices_WhenCorrectRequestPassed_ThenOkRecieved()
        {
            // given
            var expectedBundles = new List<BundleResponse>();

            // when
            var actual = _sut.GetAllServices();


            // then
            var actualResult = actual.Result as ObjectResult;

            Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
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

            _mockServicesRepository.Setup(o => o.GetService(service.Id)).Returns(service);

            // when
            var actual = _sut.DeleteService(service.Id);

            // then
            var actualResult = actual as OkResult;

            Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }



    }
}
