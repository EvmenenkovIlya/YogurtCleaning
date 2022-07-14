using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    private Mock<ILogger<BundlesController>> _logger;
    private Mock<IBundlesRepository> _bundlesRepository;
    private Mock<IBundlesService> _bundlesService;
    private Mock<IMapper> _mapper;

    [SetUp]
    public void Setup()
    {
        _bundlesRepository = new Mock<IBundlesRepository>();
        _mapper = new Mock<IMapper>();
        _sut = new BundlesController(_logger.Object, _bundlesRepository.Object, _bundlesService.Object, _mapper.Object);
    }

    //[Test]
    //public void AddBundle_WhenValidRequestPassed_ThenCreatedResultRecived()
    //{
    //    // given
    //    _bundlesRepository.Setup(b => b.AddBundle(It.IsAny<Bundle>()))
    //     .Returns(1);

    //    var bundle = new BundleRequest()
    //    {
    //        Name = "Super Bundle",
    //        Price = 10000,
    //        Measure = Measure.Room,
    //        Services = new List<ServiceResponse>()

    //    };

    //    // when

    //    // then
    //}
}
