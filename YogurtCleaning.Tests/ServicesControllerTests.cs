using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.Business.Services;
using YogurtCleaning.Controllers;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.Models;

namespace YogurtCleaning.API.Tests;

public class ServicesControllerTests
{
    private ServicesController _sut;
    private Mock<ILogger<ServicesController>> _mockLogger;
    private Mock<IServicesRepository> _mockServicesRepository;
    private Mock<IServicesService> _mockServicesService;
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
    public void GetAdditionalServicesForBundle_WhenValidRequestPassed_ThenOkResultRecived()
    {
        // given 
        var expectedServices = new List<ServiceResponse>();
        var bundle = new BundleResponse();

        // when 
        var actual = _sut.GetAdditionalServicesForBundle(bundle);


        // then 
        var actualResult = actual.Result as ObjectResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }
}
