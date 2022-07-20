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

public class BundlesControllerTests
{
    private BundlesController _sut;
    private Mock<ILogger<BundlesController>> _mockLogger;
    private Mock<IBundlesService> _mockBundlesService;
    private Mock<IMapper> _mockMapper;

    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<BundlesController>>();
        _mockBundlesService = new Mock<IBundlesService>();
        _mockMapper = new Mock<IMapper>();
        _sut = new BundlesController(_mockLogger.Object, _mockBundlesService.Object, _mockMapper.Object);
    }

    [Test]
    public void GetAdditionalServicesForBundle_WhenValidRequestPassed_ThenOkResultRecived()
    {
        // given 
        var expectedServices = new List<ServiceResponse>();
        var bundle = new BundleResponse();

        // when 
        var actual = _sut.GetAdditionalServices(bundle.Id);


        // then 
        var actualResult = actual.Result as ObjectResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }
}
