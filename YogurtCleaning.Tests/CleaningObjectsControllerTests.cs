﻿using AutoMapper;
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
using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.Tests;
public class CleaningObjectControllerTests
{
    private CleaningObjectsController _sut;
    private Mock<ICleaningObjectsService> _cleaningObjectsServiceMock;
    private IMapper _mapper;
    private UserValues _userValues;

    [SetUp]
    public void Setup()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigStorage>()));
        _cleaningObjectsServiceMock = new Mock<ICleaningObjectsService>();
        _sut = new CleaningObjectsController(_mapper, _cleaningObjectsServiceMock.Object);
        _userValues = new UserValues();
    }

    [Test]
    public async Task CreateCleaningObject_WhenValidRequestPassed_CreatedResultReceived()
    {
        //given
        int expectedId = 1;
        _cleaningObjectsServiceMock.Setup(c => c.CreateCleaningObject(It.IsAny<CleaningObject>(), It.IsAny<UserValues>()))
         .ReturnsAsync(expectedId);

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
        var actual = await _sut.AddCleaningObject(cleaningObject);

        //then
        var actualResult = actual.Result as CreatedResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That((int)actualResult.Value, Is.EqualTo(expectedId));
        _cleaningObjectsServiceMock.Verify(x => x.CreateCleaningObject(It.Is<CleaningObject>(c => c.NumberOfRooms == cleaningObject.NumberOfRooms &&
        c.NumberOfBathrooms == cleaningObject.NumberOfBathrooms && c.Square == cleaningObject.Square && c.NumberOfWindows == cleaningObject.NumberOfWindows &&
        c.NumberOfBalconies == cleaningObject.NumberOfBalconies && c.Address == cleaningObject.Address
        ), It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public async Task GetCleaningObject_WhenValidRequestPassed_OkReceived()
    {
        //given
        var expectedCleaningObject = new CleaningObject()
        {
            Id = 1,
            NumberOfRooms = 1000,
            NumberOfBathrooms = 1,
            Square = 1,
            NumberOfWindows = 1,
            NumberOfBalconies = 0,
            Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
        };

        _cleaningObjectsServiceMock.Setup(o => o.GetCleaningObject(expectedCleaningObject.Id, It.IsAny<UserValues>())).ReturnsAsync(expectedCleaningObject);

        //when
        var actual = await _sut.GetCleaningObject(expectedCleaningObject.Id);

        //then

        var actualResult = actual.Result as ObjectResult;
        var cleaningObjectResponse = actualResult.Value as CleaningObjectResponse;
        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.Multiple(() =>
        {
            Assert.That(expectedCleaningObject.NumberOfRooms, Is.EqualTo(cleaningObjectResponse.NumberOfRooms));
            Assert.That(expectedCleaningObject.NumberOfBathrooms, Is.EqualTo(cleaningObjectResponse.NumberOfBathrooms));
            Assert.That(expectedCleaningObject.Square, Is.EqualTo(cleaningObjectResponse.Square));
            Assert.That(expectedCleaningObject.NumberOfWindows, Is.EqualTo(cleaningObjectResponse.NumberOfWindows));
            Assert.That(expectedCleaningObject.Address, Is.EqualTo(cleaningObjectResponse.Address));
        });
        _cleaningObjectsServiceMock.Verify(x => x.GetCleaningObject(expectedCleaningObject.Id, It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public async Task UpdateCleaningObject_WhenValidRequestPassed_NoContentReceived()
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
            Client = new Client() { Id = 1},
            District = new District() { Id = DistrictEnum.Admiralteisky}
        };

        var newCleaningObjectModel = new CleaningObjectUpdateRequest()
        {
            NumberOfRooms = 10,
            NumberOfBathrooms = 100,
            Square = 7,
            NumberOfWindows = 3,
            NumberOfBalconies = 8,
            Address = "г. Санкт-Петербург, ул. Льва Толстого, д. 16, кв. 10",
            District = DistrictEnum.Admiralteisky
        };

        _cleaningObjectsServiceMock.Setup(o => o.UpdateCleaningObject(cleaningObject, cleaningObject.Id, _userValues));

        //when
        var actual = await _sut.UpdateCleaningObject(newCleaningObjectModel, cleaningObject.Id);

        //then
        var actualResult = actual as NoContentResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        _cleaningObjectsServiceMock.Verify(c => c.UpdateCleaningObject(It.Is<CleaningObject>(c => c.NumberOfRooms == newCleaningObjectModel.NumberOfRooms &&
        c.NumberOfBathrooms == newCleaningObjectModel.NumberOfBathrooms && c.Square == newCleaningObjectModel.Square &&
        c.NumberOfWindows == newCleaningObjectModel.NumberOfWindows && c.NumberOfBalconies == newCleaningObjectModel.NumberOfBalconies &&
        c.Address == newCleaningObjectModel.Address), It.Is<int>(i => i == cleaningObject.Id), It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public async Task DeleteCleaningObjectById_WhenValidRequestPassed_NoContentReceived()
    {
        //given
        var expectedCleaningObject = new CleaningObject()
        {
            Id = 1,
            NumberOfRooms = 10,
            NumberOfBathrooms = 100,
            NumberOfBalconies = 8,
            Address = "г. Санкт-Петербург, ул. Льва Толстого, д. 16, кв. 10",
            IsDeleted = false
        };

        //when
        var actual = await _sut.DeleteCleaningObject(expectedCleaningObject.Id);

        //then
        var actualResult = actual as NoContentResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        _cleaningObjectsServiceMock.Verify(c => c.DeleteCleaningObject(It.IsAny<int>(), It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public async Task GetAllCleaningObject_WhenValidRequestPassed_RequestedTypeReceived()
    {
        //given
        var client = new Client() { Id = 1 };
        var cleaningObjects = new List<CleaningObject>
        {
            new CleaningObject()
            {
                Id = 1,
                NumberOfRooms = 1000,
                NumberOfBathrooms = 1,
                Square = 1,
                NumberOfWindows = 1,
                NumberOfBalconies = 0,
                Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
                Client = client,
                District = new District() { Id = DistrictEnum.Kalininsky}
            },
            new CleaningObject()
            {
                Id = 2,
                NumberOfRooms = 10,
                NumberOfBathrooms = 9,
                Square = 2,
                NumberOfWindows = 5,
                NumberOfBalconies = 5,
                Address = "г. Смоленск, ул. Льва Толстого, д. 16, кв. 10",
                Client = client,
                District = new District() { Id = DistrictEnum.Admiralteisky}
            },
        };

        _cleaningObjectsServiceMock.Setup(o => o.GetAllCleaningObjectsByClientId(client.Id, It.IsAny<UserValues>())).ReturnsAsync(cleaningObjects).Verifiable();

        //when
        var actual = await _sut.GetAllCleaningObjectsByClientId(client.Id);

        //then
        var actualResult = actual.Result as ObjectResult;
        var cleaningObjectResponse = actualResult.Value as List<CleaningObjectResponse>;


        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.Multiple(() =>
        {
            Assert.That(cleaningObjectResponse.Count, Is.EqualTo(cleaningObjects.Count));
            Assert.That(cleaningObjectResponse[0].NumberOfRooms, Is.EqualTo(cleaningObjects[0].NumberOfRooms));
            Assert.That(cleaningObjectResponse[1].NumberOfBathrooms, Is.EqualTo(cleaningObjects[1].NumberOfBathrooms));
            Assert.That(cleaningObjectResponse[0].Square, Is.EqualTo(cleaningObjects[0].Square));
            Assert.That(cleaningObjectResponse[1].NumberOfWindows, Is.EqualTo(cleaningObjects[1].NumberOfWindows));
            Assert.That(cleaningObjectResponse[0].NumberOfBalconies, Is.EqualTo(cleaningObjects[0].NumberOfBalconies));
            Assert.That(cleaningObjectResponse[1].Address, Is.EqualTo(cleaningObjects[1].Address));
            Assert.That(cleaningObjectResponse[0].ClientId, Is.EqualTo(cleaningObjects[0].Client.Id));
            Assert.That(cleaningObjectResponse[0].District, Is.EqualTo(cleaningObjects[0].District.Id));
        });
        _cleaningObjectsServiceMock.Verify(x => x.GetAllCleaningObjectsByClientId(client.Id, It.IsAny<UserValues>()), Times.Once);
    }
}