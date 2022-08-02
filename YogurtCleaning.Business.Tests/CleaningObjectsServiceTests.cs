﻿using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Facts;

public class CleaningObjectServiceFacts
{
    private CleaningObjectsService _sut;
    private Mock<ICleaningObjectsRepository> _cleaningObjectsRepositoryMock;
    private Mock<IClientsRepository> _clientsRepositoryMock;
    private UserValues _userValues;
    
    public CleaningObjectServiceFacts()
    {
        _cleaningObjectsRepositoryMock = new Mock<ICleaningObjectsRepository>();
        _clientsRepositoryMock = new Mock<IClientsRepository>();
        _sut = new CleaningObjectsService(_cleaningObjectsRepositoryMock.Object, _clientsRepositoryMock.Object);
    }

    [Fact]
    public void CreateCleaningObject_WhenValidRequestPassed_CleaningObjectAdded()
    {
        //given   
        int expectedId = 1;
        _cleaningObjectsRepositoryMock.Setup(c => c.CreateCleaningObject(It.IsAny<CleaningObject>()))
             .Returns(expectedId);
        var expectedClient = new Client() { Id = 1 };
        var expectedDistrict = new District() { Id = DistrictEnum.Vasileostrovskiy };
        var cleaningObject = new CleaningObject()
        {
            NumberOfRooms = 1000,
            NumberOfBathrooms = 1,
            Square = 1,
            NumberOfWindows = 1,
            NumberOfBalconies = 0,
            Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
            Client = new Client() { Id = 1 },
            District = new District() { Id = DistrictEnum.Vasileostrovskiy},
            IsDeleted = false
        };
        _clientsRepositoryMock.Setup(c => c.GetClient(cleaningObject.Client.Id)).Returns(expectedClient);
        _cleaningObjectsRepositoryMock.Setup(c => c.GetDistrict(cleaningObject.District.Id)).Returns(expectedDistrict);
        UserValues userValues = new UserValues() { Id = expectedId };

        //when
        var actual = _sut.CreateCleaningObject(cleaningObject, userValues);

        //then
        Assert.True(actual == expectedId);
        _cleaningObjectsRepositoryMock.Verify(c => c.CreateCleaningObject(cleaningObject), Times.Once);
        _cleaningObjectsRepositoryMock.Verify(c => c.CreateCleaningObject(It.Is<CleaningObject>(c => c.District.Id == cleaningObject.District.Id)), Times.Once);
        _cleaningObjectsRepositoryMock.Verify(c => c.CreateCleaningObject(It.Is<CleaningObject>(c => c.Client.Id == userValues.Id)), Times.Once);
    }

    [Fact]
    public void GetCleaningObject_WhenCurrentUserIsAdmin_CleaningObjectReceived()
    {
        //given
        var cleaningObjectInDb = new CleaningObject()
        {
            Id = 1,
            NumberOfRooms = 1000,
            NumberOfBathrooms = 1,
            Square = 1,
            NumberOfWindows = 1,
            NumberOfBalconies = 0,
            Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
            Client = new Client() { Id = 2 },
            IsDeleted = false
        };

        _userValues = new UserValues() { Email = "AdamSmith@gmail.com1", Role = Role.Admin, Id = 1 };
        _cleaningObjectsRepositoryMock.Setup(o => o.GetCleaningObject(cleaningObjectInDb.Id)).Returns(cleaningObjectInDb);

        //when
        var actual = _sut.GetCleaningObject(cleaningObjectInDb.Id, _userValues);

        //then
        _cleaningObjectsRepositoryMock.Verify(c => c.GetCleaningObject(cleaningObjectInDb.Id), Times.Once);
    }

    [Fact]
    public void GetCleaningObject_WhenIdNotInBase_GetEntityNotFoundException()
    {
        //given
        var testId = 2;
        _userValues = new UserValues() { Role = Role.Admin };

        //when

        //then
        Assert.Throws<Exceptions.EntityNotFoundException>(() => _sut.GetCleaningObject(testId, _userValues));
    }

    [Fact]
    public void GetCleaningObject_WhenClientGetSomeoneElsesCleaningObject_ThrowAccessException()
    {
        //given
        var cleaningObjectInDb = new CleaningObject()
        {
            Id = 1,
            Client = new Client() { Id = 1},
            NumberOfRooms = 1000,
            NumberOfBathrooms = 1,
            Square = 1,
            NumberOfWindows = 1,
            NumberOfBalconies = 0,
            Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
            IsDeleted = false
        };
        _userValues = new UserValues() { Role = Role.Client, Id = 2 };
        _cleaningObjectsRepositoryMock.Setup(o => o.GetCleaningObject(cleaningObjectInDb.Id)).Returns(cleaningObjectInDb);

        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.GetCleaningObject(cleaningObjectInDb.Id, _userValues));
    }


    [Fact]
    public void UpdateCleaningObject_WhenUserUpdatesOwnCleaningObjectProperties_ChangesProperties()
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
            Client = new Client() { Id = 1 }
        };

        CleaningObject newCleaningObjectModel = new CleaningObject()
        {
            NumberOfRooms = 10,
            NumberOfBathrooms = 100,
            Square = 7,
            NumberOfWindows = 3,
            NumberOfBalconies = 8,
            Address = "г. Санкт-Петербург, ул. Льва Толстого, д. 16, кв. 10",
        };
        _userValues = new UserValues() { Id = cleaningObject.Client.Id };
        _cleaningObjectsRepositoryMock.Setup(o => o.GetCleaningObject(cleaningObject.Id)).Returns(cleaningObject);
        _cleaningObjectsRepositoryMock.Setup(o => o.UpdateCleaningObject(newCleaningObjectModel));

        //when
        _sut.UpdateCleaningObject(newCleaningObjectModel, cleaningObject.Id, _userValues);

        //then
        _cleaningObjectsRepositoryMock.Verify(c => c.GetCleaningObject(cleaningObject.Id), Times.Once);
        _cleaningObjectsRepositoryMock.Verify(c => c.UpdateCleaningObject(It.Is<CleaningObject>(c => c.NumberOfRooms == newCleaningObjectModel.NumberOfRooms &&
        c.NumberOfBathrooms == newCleaningObjectModel.NumberOfBathrooms && c.Square == newCleaningObjectModel.Square &&
        c.NumberOfWindows == newCleaningObjectModel.NumberOfWindows && c.NumberOfBalconies == newCleaningObjectModel.NumberOfBalconies &&
        c.Address == newCleaningObjectModel.Address)),
        Times.Once);
    }

    [Fact]
    public void UpdateCleaningObject_WhenAdminUpdatesOwnCleaningObjectProperties_ChangesProperties()
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
            Client = new Client() { Id = 1 }
        };

        CleaningObject newCleaningObjectModel = new CleaningObject()
        {
            NumberOfRooms = 10,
            NumberOfBathrooms = 100,
            Square = 7,
            NumberOfWindows = 3,
            NumberOfBalconies = 8,
            Address = "г. Санкт-Петербург, ул. Льва Толстого, д. 16, кв. 10",
        };
        _userValues = new UserValues() { Role = Role.Admin };
        _cleaningObjectsRepositoryMock.Setup(o => o.GetCleaningObject(cleaningObject.Id)).Returns(cleaningObject);
        _cleaningObjectsRepositoryMock.Setup(o => o.UpdateCleaningObject(newCleaningObjectModel));

        //when
        _sut.UpdateCleaningObject(newCleaningObjectModel, cleaningObject.Id, _userValues);

        //then
        _cleaningObjectsRepositoryMock.Verify(c => c.GetCleaningObject(cleaningObject.Id), Times.Once);
        _cleaningObjectsRepositoryMock.Verify(c => c.UpdateCleaningObject(It.Is<CleaningObject>(c => c.NumberOfRooms == newCleaningObjectModel.NumberOfRooms &&
        c.NumberOfBathrooms == newCleaningObjectModel.NumberOfBathrooms && c.Square == newCleaningObjectModel.Square &&
        c.NumberOfWindows == newCleaningObjectModel.NumberOfWindows && c.NumberOfBalconies == newCleaningObjectModel.NumberOfBalconies &&
        c.Address == newCleaningObjectModel.Address)),
        Times.Once);
    }

    [Fact]
    public void UpdateCleaningObject_WhenEmptyCleaningObjectRequest_ThrowEntityNotFoundException()
    {
        //given
        var cleaningObject = new CleaningObject() { Client = new Client() { Id = 1 } };

        CleaningObject newCleaningObjectModel = new CleaningObject()
        {
            NumberOfRooms = 10,
            NumberOfBathrooms = 100,
            Square = 7,
        };
        _userValues = new UserValues() { Id = cleaningObject.Client.Id };

        _cleaningObjectsRepositoryMock.Setup(o => o.UpdateCleaningObject(newCleaningObjectModel));

        //when

        //then
        Assert.Throws<Exceptions.BadRequestException>(() => _sut.UpdateCleaningObject(newCleaningObjectModel, cleaningObject.Id, _userValues));
    }

    [Fact]
    public void UpdateCleaningObject_UserTryUpdateSomeoneElseCleaningObject_ThrowAccessException()
    {
        //given
        var testEmail = "FakeCleaningObject@gmail.ru";

        var cleaningObject = new CleaningObject()
        {
            Id = 1,
            NumberOfRooms = 1000,
            Client = new Client() { Id = 1 }
        };

        CleaningObject newCleaningObjectModel = new CleaningObject()
        {
            NumberOfRooms = 10
        };
        _userValues = new UserValues() { Id = 2 };
        _cleaningObjectsRepositoryMock.Setup(o => o.GetCleaningObject(cleaningObject.Id)).Returns(cleaningObject);
        _cleaningObjectsRepositoryMock.Setup(o => o.UpdateCleaningObject(newCleaningObjectModel));

        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.UpdateCleaningObject(newCleaningObjectModel, cleaningObject.Id, _userValues));
    }

    [Fact]
    public void DeleteCleaningObject_WhenValidRequestPassed_DeleteCleaningObject()
    {
        //given
        var expectedCleaningObject = new CleaningObject()
        {
            Id = 1,
            NumberOfRooms = 10,
            NumberOfBathrooms = 100,
            Square = 7,
            NumberOfWindows = 3,
            NumberOfBalconies = 8,
            Address = "г. Санкт-Петербург, ул. Льва Толстого, д. 16, кв. 10",
            Client = new Client() { Id = 1 },
            IsDeleted = false
        };

        _cleaningObjectsRepositoryMock.Setup(o => o.GetCleaningObject(expectedCleaningObject.Id)).Returns(expectedCleaningObject);
        _cleaningObjectsRepositoryMock.Setup(o => o.DeleteCleaningObject(expectedCleaningObject));
        _userValues = new UserValues() { Email = "AdamSmith@gmail.com3", Role = Role.Client, Id = 1 };

        //when
        _sut.DeleteCleaningObject(expectedCleaningObject.Id, _userValues);

        //then
        _cleaningObjectsRepositoryMock.Verify(c => c.DeleteCleaningObject(expectedCleaningObject), Times.Once);
    }

    [Fact]
    public void DeleteCleaningObject_EmptyCleaningObjectRequest_ThrowEntityNotFoundException()
    {
        //given
        var testId = 1;
        var cleaningObject = new CleaningObject();
        var testEmail = "FakeCleaningObject@gmail.ru";
        _userValues = new UserValues() { Email = testEmail, Role = Role.Client };

        //when

        //then
        Assert.Throws<Exceptions.BadRequestException>(() => _sut.DeleteCleaningObject(testId, _userValues));
    }

    [Fact]
    public void DeleteCleaningObject_WhenClientDeleteSomeoneElsesCleaningObject_ThrowAccessException()
    {
        //given
        int clientId = 1;
        var cleaningObject = new CleaningObject()
        {
            Id = 1,
            NumberOfRooms = 10,
            NumberOfBathrooms = 100,
            Square = 7,
            NumberOfWindows = 3,
            NumberOfBalconies = 8,
            Address = "г. Санкт-Петербург, ул. Льва Толстого, д. 16, кв. 10",
            Client = new Client() { Id = 2 },
            IsDeleted = false

        };
        _userValues = new UserValues() { Email = cleaningObject.Client.Email, Role = Role.Client };
        _cleaningObjectsRepositoryMock.Setup(o => o.GetCleaningObject(cleaningObject.Id)).Returns(cleaningObject);

        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.DeleteCleaningObject(cleaningObject.Id, _userValues));
    }
}