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
    public void UpdateCleaningObject_WhenValidRequestPassed_NoContentReceived()
    {
        //given

        var cleaningObject = new CleaningObject()
        {
            Id = 1,
            NumberOfRooms = 1000,
            NumberOfBathrooms = 1,
            Square = 1,
            NumberOfWindows = 1,
            NumberOfBalconies = 0,
            Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
            Client = new Client() { Id = 1}
        };

        var newCleaningObjectModel = new CleaningObjectUpdateRequest()
        {
            NumberOfRooms = 10,
            NumberOfBathrooms = 100,
            Square = 7,
            NumberOfWindows = 3,
            NumberOfBalconies = 8,
            Address = "г. Санкт-Петербург, ул. Льва Толстого, д. 16, кв. 10",
        };

        _cleaningObjectsServiceMock.Setup(o => o.UpdateCleaningObject(cleaningObject, cleaningObject.Id, _userValues));

        //when
        var actual = _sut.UpdateCleaningObject(newCleaningObjectModel, cleaningObject.Id);

        //then
        var actualResult = actual as NoContentResult;

        Assert.AreEqual(StatusCodes.Status204NoContent, actualResult.StatusCode);
        _cleaningObjectsServiceMock.Verify(c => c.UpdateCleaningObject(It.Is<CleaningObject>(c => c.NumberOfRooms == newCleaningObjectModel.NumberOfRooms &&
        c.NumberOfBathrooms == newCleaningObjectModel.NumberOfBathrooms && c.Square == newCleaningObjectModel.Square &&
        c.NumberOfWindows == newCleaningObjectModel.NumberOfWindows && c.NumberOfBalconies == newCleaningObjectModel.NumberOfBalconies &&
        c.Address == newCleaningObjectModel.Address), It.Is<int>(i => i == cleaningObject.Id), It.IsAny<UserValues>()), Times.Once);
    }
}