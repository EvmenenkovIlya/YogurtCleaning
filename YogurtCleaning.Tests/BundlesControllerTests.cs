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

namespace YogurtCleaning.API.Tests;

public class BundlesControllerTests
{
    private BundlesController _sut;
    private Mock<ILogger<BundlesController>> _mockLogger;
    private Mock<IBundlesRepository> _mockBundlesRepository;
    private Mock<IBundlesService> _mockBundlesService;
    private Mock<IMapper> _mockMapper;

    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<BundlesController>>();
        _mockBundlesRepository = new Mock<IBundlesRepository>();
        _mockBundlesService = new Mock<IBundlesService>();
        _mockMapper = new Mock<IMapper>();
        _sut = new BundlesController(_mockLogger.Object, _mockBundlesRepository.Object, _mockBundlesService.Object, _mockMapper.Object);
    }

    [Test]
    public void AddBundle_WhenValidRequestPassed_ThenCreatedResultRecived()
    {
        // given
        _mockBundlesRepository.Setup(o => o.AddBundle(It.IsAny<Bundle>())).Returns(1);
        var bundle = new BundleRequest()
        {
            Name = "Super Bundle",
            Type = CleaningType.Regular,
            Price = 10000,
            Measure = Measure.Room,
            Services = new List<ServiceResponse>()
        };

        // when
        var actual = _sut.AddBundle(bundle);

        // then
        var actualResult = actual.Result as CreatedResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.True((int)actualResult.Value == 1);
    }

    [Test]
    public void GetBundle_WhenCorrectIdPassed_ThenOkRecieved()
    {
        // given
        var expectedBundle = new Bundle()
        {
            Id = 2,
            Name = "Super Bundle",
            Type = CleaningType.Regular,
            Price = 10000,
            Measure = Measure.Room,
            Services = new List<Service>()
        };
        _mockBundlesRepository.Setup(o => o.GetBundle(expectedBundle.Id)).Returns(expectedBundle);

        // when
        var actual = _sut.GetBundle(expectedBundle.Id);

        // then
        var actualResult = actual.Result as ObjectResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

    }

    [Test]
    public void GetAllBundles_WhenCorrectRequestPassed_ThenOkRecieved()
    {
        // given
        var expectedBundles = new List<BundleResponse>();

        // when
        var actual = _sut.GetAllBundles();


        // then
        var actualResult = actual.Result as ObjectResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public void UpdateBundle_WhenCorrectRequestPassed_ThenNoContentRecieved()
    {
        // given
        var bundle = new Bundle()
        {
            Id = 3,
            Name = "Super Bundle",
            Type = CleaningType.Regular,
            Price = 10000,
            Measure = Measure.Room,
            Services = new List<Service>()
        };

        var newProperty = new BundleRequest()
        {
            Name = "Crazy Bundle",
            Price = 999999
        };

        _mockBundlesRepository.Setup(o => o.UpdateBundle(bundle));

        // when
        var actual = _sut.UpdateBundle(newProperty, bundle.Id);

        // then
        var actualResult = actual as NoContentResult;
        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
    }

    [Test]
    public void DeleteBundle_WhenCorrectRequestPassed_ThenOkRecieved()
    {
        // given
        var bundle = new Bundle()
        {
            Id = 4,
            Name = "Super Bundle",
            Type = CleaningType.Regular,
            Price = 10000,
            Measure = Measure.Room,
            Services = new List<Service>()
        };

        _mockBundlesRepository.Setup(o => o.GetBundle(bundle.Id)).Returns(bundle);

        // when
        var actual = _sut.DeleteBundle(bundle.Id);

        // then
        var actualResult = actual as NoContentResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
    }
}
