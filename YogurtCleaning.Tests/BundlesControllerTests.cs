using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.Controllers;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Models;

namespace YogurtCleaning.API.Tests;

public class BundlesControllerTests
{
    private BundlesController _sut;
    private Mock<IBundlesService> _mockBundlesService;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {      
        _mockBundlesService = new Mock<IBundlesService>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigStorage>()));
        _sut = new BundlesController(_mockBundlesService.Object, _mapper);
    }

    [Test]
    public async Task AddBundle_WhenValidRequestPassed_ThenCreatedResultRecived()
    {
        // given
        int expectedId = 1;
        _mockBundlesService.Setup(o => o.AddBundle(It.IsAny<Bundle>())).ReturnsAsync(expectedId);

        var bundle = new BundleRequest()
        {
            Name = "Super Bundle",
            Type = CleaningType.Regular,
            Price = 10000,
            Measure = Measure.Room,
            Services = new List<ServiceResponse>()
        };

        // when
        var actual = await _sut.AddBundle(bundle);

        // then
        var actualResult = actual.Result as CreatedResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That((int)actualResult.Value, Is.EqualTo(expectedId)); 
        _mockBundlesService.Verify(o => o.AddBundle(It.IsAny<Bundle>()), Times.Once);
    }

    [Test]
    public async Task GetBundle_WhenCorrectIdPassed_ThenOkRecieved()
    {
        // given
        var expectedBundle = new Bundle()
        {
            Id = 2,
            Name = "Super Bundle",
            Type = CleaningType.Regular,
            Price = 10000,
            Measure = Measure.Room,
            Services = new List<Service>() { new Service(), new Service()}
        };
        _mockBundlesService.Setup(o => o.GetBundle(expectedBundle.Id)).ReturnsAsync(expectedBundle);

        // when
        var actual = await _sut.GetBundle(expectedBundle.Id);

        // then
        var actualResult = actual.Result as ObjectResult;
        var bundleResponse = actualResult.Value as BundleResponse;
        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.Multiple(() =>
        {
            Assert.That(bundleResponse.Name, Is.EqualTo(expectedBundle.Name));
            Assert.That(bundleResponse.Type, Is.EqualTo(expectedBundle.Type));
            Assert.That(bundleResponse.Price, Is.EqualTo(expectedBundle.Price));
            Assert.That(bundleResponse.Measure, Is.EqualTo(expectedBundle.Measure));
            Assert.That(bundleResponse.Services.Count, Is.EqualTo(expectedBundle.Services.Count));
        });
        _mockBundlesService.Verify(o => o.GetBundle(expectedBundle.Id), Times.Once);
    }

    [Test]
    public async Task GetAllBundles_WhenCorrectRequestPassed_ThenOkRecieved()
    {
        // given
        var expectedBundles = new List<BundleResponse>();

        // when
        var actual = await _sut.GetAllBundles();


        // then
        var actualResult = actual.Result as ObjectResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

    [Test]
    public async Task UpdateBundle_WhenCorrectRequestPassed_ThenNoContentRecieved()
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

        _mockBundlesService.Setup(o => o.UpdateBundle(bundle, bundle.Id));

        // when
        var actual = await _sut.UpdateBundle(newProperty, bundle.Id);

        // then
        var actualResult = actual as NoContentResult;
        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
    }

    [Test]
    public async Task DeleteBundle_WhenCorrectRequestPassed_ThenOkRecieved()
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

        _mockBundlesService.Setup(o => o.GetBundle(bundle.Id)).ReturnsAsync(bundle);

        // when
        var actual = await _sut.DeleteBundle(bundle.Id);

        // then
        var actualResult = actual as NoContentResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        _mockBundlesService.Verify(o => o.DeleteBundle(bundle.Id), Times.Once);
    }

    [Test]
    public async Task GetAdditionalServicesForBundle_WhenValidRequestPassed_ThenOkResultRecived()
    {
        // given 
        var bundle = new BundleResponse();

        // when 
        var actual = await _sut.GetAdditionalServices(bundle.Id);


        // then 
        var actualResult = actual.Result as ObjectResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }
}

    