using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.Controllers;
using YogurtCleaning.Models;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.Business;
using YogurtCleaning.API;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Tests;
public class CleaningObjectsControllerTests
{
    private CleaningObjectsController _sut;
    private Mock<ICleaningObjectsService> _cleaningObjectsServiceMock;
    private readonly ICleaningObjectsRepository _cleaningObjectsRepository;
    private IMapper _mapper;
    private UserValues _userValues;

    [SetUp]
    public void Setup()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigStorage>()));
        _cleaningObjectsServiceMock = new Mock<ICleaningObjectsService>();
        _sut = new CleaningObjectsController(_cleaningObjectsRepository, _mapper, _cleaningObjectsServiceMock.Object);
        _userValues = new UserValues();
    }

    [Test]
    public async Task CreateCleaningObject_WhenValidRequestPassed_CreatedResultReceived()
    {
        //given
        _cleaningObjectsServiceMock.Setup(c => c.CreateCleaningObject(It.IsAny<CleaningObject>(), It.IsAny<UserValues>()))
         .Returns(1);

        var cleaningObject = new CleaningObjectRequest()
        {
            NumberOfRooms = 1000,
            NumberOfBathrooms = 1,
            Square = 1,
            NumberOfWindows = 1,
            NumberOfBalconies = 0,
            Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
        };

        //when
        var actual = _sut.AddCleaningObject(cleaningObject);

        //then
        var actualResult = actual.Result as CreatedResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That((int)actualResult.Value, Is.EqualTo(1));
        _cleaningObjectsServiceMock.Verify(x => x.CreateCleaningObject(It.Is<CleaningObject>(c => c.NumberOfRooms == cleaningObject.NumberOfRooms &&
        c.NumberOfBathrooms == cleaningObject.NumberOfBathrooms && c.Square == cleaningObject.Square && c.NumberOfWindows == cleaningObject.NumberOfWindows &&
        c.NumberOfBalconies == cleaningObject.NumberOfBalconies && c.Address == cleaningObject.Address
        ), It.IsAny<UserValues>()), Times.Once);
    }
}