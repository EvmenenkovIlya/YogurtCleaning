using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.Controllers;
using YogurtCleaning.Models;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.Business;

namespace YogurtCleaning.Tests;
public class ClientsControllerTests
{
    private ClientsController _sut;
    private Mock<IClientsService> _clientsServiceMock;
    private Mock<IMapper> _mapper;

    private UserValues _userValues;

    [SetUp]
    public void Setup()
    {
        _mapper = new Mock<IMapper>();
        _clientsServiceMock = new Mock<IClientsService>();
        _sut = new ClientsController(_mapper.Object, _clientsServiceMock.Object);
    }

    [Test]
    public async Task CreateClient_WhenValidRequestPassed_CreatedResultReceived()
    {
        //given
        _clientsServiceMock.Setup(c => c.CreateClient(It.IsAny<Client>()))
         .Returns(1);

        var client = new ClientRegisterRequest()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            ConfirmPassword = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            BirthDate = DateTime.Today
        };

        //when
        var actual = _sut.AddClient(client);
        var a = actual.Result;

        //then
        var actualResult = actual.Result as CreatedResult;

        Assert.AreEqual(StatusCodes.Status201Created, actualResult.StatusCode);
        Assert.True((int)actualResult.Value == 1);

        _clientsServiceMock.Verify(c => c.CreateClient(It.IsAny<Client>()), Times.Once);
    }

    [Test]
    public void GetClient_WhenValidRequestPassed_OkReceived()
    {
        //given
        var expectedClient = new Client()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            BirthDate = DateTime.Today
        };
        _clientsServiceMock.Setup(o => o.GetClient(expectedClient.Id, _userValues)).Returns(expectedClient);

        //when
        var actual = _sut.GetClient(expectedClient.Id);

        //then
        var actualResult = actual.Result as ObjectResult;


        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        _clientsServiceMock.Verify(c => c.GetClient(It.IsAny<int>(), It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public void UpdateClient_WhenValidRequestPassed_NoContentReceived()
    {
        //given

        var client = new Client()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            BirthDate = DateTime.Today
        };


        var newClientModel = new ClientUpdateRequest()
        {
            FirstName = "Valid",
            LastName = "Person",
            Phone = "893048827534",
            BirthDate = DateTime.Today
        };

        _clientsServiceMock.Setup(o => o.UpdateClient(client, client.Id, _userValues));

        //when
        var actual = _sut.UpdateClient(newClientModel, client.Id);

        //then
        var actualResult = actual as NoContentResult;

        Assert.AreEqual(StatusCodes.Status204NoContent, actualResult.StatusCode);
        _clientsServiceMock.Verify(c => c.UpdateClient(It.IsAny<Client>(), It.IsAny<int>(), It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public void GetCommentsByClientId_WhenValidRequestPassed_RequestedTypeReceived()
    {
        //given
        var expectedClient = new Client()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
            Comments = new()
            {
                new()
                {
                    Id = 1, Summary = "best cleaners", Rating = 5
                },
                new()
                {
                    Id = 2, Summary = "bad cleaners", Rating = 2
                }
            },

        };
        _clientsServiceMock.Setup(o => o.GetCommentsByClient(expectedClient.Id, _userValues)).Returns(expectedClient.Comments);

        //when
        var actual = _sut.GetAllCommentsByClient(expectedClient.Id);

        //then
        var actualResult = actual.Result as ObjectResult;

        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        _clientsServiceMock.Verify(c => c.GetCommentsByClient(It.IsAny<int>(), It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public void GetOrdersByClient_WhenValidRequestPassed_RequestedTypeReceived()
    {
        //given

        var expectedClient = new Client()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
            Orders = new()
            {
                new()
                {
                    Id = 1, Price = 124, Status = DataLayer.Enums.Status.Created
                },
                new()
                {
                    Id = 2, Price = 1245, Status = DataLayer.Enums.Status.Done
                }
            },

        };

        _clientsServiceMock.Setup(o => o.GetOrdersByClient(expectedClient.Id, _userValues)).Returns(expectedClient.Orders);

        //when
        var actual = _sut.GetAllOrdersByClient(expectedClient.Id);

        //then
        var actualResult = actual.Result as ObjectResult;

        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        _clientsServiceMock.Verify(c => c.GetOrdersByClient(It.IsAny<int>(), It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public void DeleteClientById_WhenValidRequestPassed_NoContentReceived()
    {
        //given
        var expectedClient = new Client()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
            IsDeleted = false

        };

        _clientsServiceMock.Setup(o => o.GetClient(expectedClient.Id, _userValues)).Returns(expectedClient);

        //when
        var actual = _sut.DeleteClient(expectedClient.Id);

        //then
        var actualResult = actual as NoContentResult;

        Assert.AreEqual(StatusCodes.Status204NoContent, actualResult.StatusCode);
        _clientsServiceMock.Verify(c => c.DeleteClient(It.IsAny<int>(), It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public void GetAllClients_WhenValidRequestPassed_RequestedTypeReceived()
    {
        //given
        var clients = new List<Client>
        {
            new Client()
            {
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com",
                Phone = "85559997264",
                BirthDate = DateTime.Today
            },
            new Client()
            {
                FirstName = "Adam1",
                LastName = "Smith1",
                Password = "123456781",
                Email = "AdamSmith@gmail.com1",
                Phone = "855599972641",
                BirthDate = DateTime.Today,
                IsDeleted = true,
            },
            new Client()
            {
                FirstName = "Adam2",
                LastName = "Smith2",
                Password = "123456782",
                Email = "AdamSmith@gmail.com2",
                Phone = "855599972642",
                BirthDate = DateTime.Today,
            }
        };

        _clientsServiceMock.Setup(o => o.GetAllClients(_userValues)).Returns(clients).Verifiable();

        //when
        var actual = _sut.GetAllClients();

        //then
        var actualResult = actual.Result as ObjectResult;

        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        _clientsServiceMock.Verify(c => c.GetAllClients(It.IsAny<UserValues>()), Times.Once);
    }
}