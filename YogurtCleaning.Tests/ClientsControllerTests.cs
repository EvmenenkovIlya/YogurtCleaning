using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.Controllers;
using YogurtCleaning.Models;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.Business;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.API;

namespace YogurtCleaning.Tests;
public class ClientsControllerTests
{
    private ClientsController _sut;
    private Mock<IClientsService> _clientsServiceMock;
    private IMapper _mapper;
    private UserValues _userValues;

    [SetUp]
    public void Setup()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigStorage>()));
        _clientsServiceMock = new Mock<IClientsService>();
        _sut = new ClientsController(_mapper, _clientsServiceMock.Object);
        _userValues = new UserValues();
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

        //then
        var actualResult = actual.Result as CreatedResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That((int)actualResult.Value, Is.EqualTo(1));
        _clientsServiceMock.Verify(x => x.CreateClient(It.Is<Client>(c => 
        c.FirstName == client.FirstName &&
        c.LastName == client.LastName &&
        c.Password == client.Password &&
        c.Email == client.Email &&
        c.Phone == client.Phone &&
        c.BirthDate == client.BirthDate
        )), Times.Once);
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
        
        _clientsServiceMock.Setup(o => o.GetClient(expectedClient.Id, It.IsAny<UserValues>())).Returns(expectedClient);

        //when
        var actual = _sut.GetClient(expectedClient.Id);

        //then
        
        var actualResult = actual.Result as ObjectResult;
        var clientResponce = actualResult.Value as ClientResponse;
        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        Assert.True(clientResponce.FirstName == expectedClient.FirstName);
        Assert.True(clientResponce.LastName == expectedClient.LastName);
        Assert.True(clientResponce.Email == expectedClient.Email);
        Assert.True(clientResponce.Phone == expectedClient.Phone);
        Assert.True(clientResponce.BirthDate == expectedClient.BirthDate);
        _clientsServiceMock.Verify(x => x.GetClient(expectedClient.Id, It.IsAny<UserValues>()), Times.Once);
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
        _clientsServiceMock.Verify(c => c.UpdateClient(It.Is<Client>(c => 
        c.FirstName == newClientModel.FirstName &&
        c.LastName == newClientModel.LastName &&
        c.Email == null &&
        c.BirthDate == newClientModel.BirthDate &&
        c.Phone == newClientModel.Phone
        ), It.Is<int>(i => i == client.Id), It.IsAny<UserValues>()), Times.Once);        
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
        _clientsServiceMock.Setup(o => o.GetCommentsByClient(expectedClient.Id, It.IsAny<UserValues>())).Returns(expectedClient.Comments);

        //when
        var actual = _sut.GetAllCommentsByClient(expectedClient.Id);

        //then
        var actualResult = actual.Result as ObjectResult;
        var commentsResponse = actualResult.Value as List<CommentResponse>;

        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        Assert.True(expectedClient.Comments.Count == commentsResponse.Count);
        Assert.True(expectedClient.Comments[0].Id == commentsResponse[0].Id);
        Assert.True(expectedClient.Comments[1].Summary == commentsResponse[1].Summary);
        Assert.True(expectedClient.Comments[0].Rating == commentsResponse[0].Rating);
        _clientsServiceMock.Verify(c => c.GetCommentsByClient(expectedClient.Id, It.IsAny<UserValues>()), Times.Once);
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
                    Id = 1, Price = 124, Status = Status.Created
                },
                new()
                {
                    Id = 2, Price = 1245, Status = Status.Done
                }
            },
        };

        _clientsServiceMock.Setup(o => o.GetOrdersByClient(expectedClient.Id, It.IsAny<UserValues>())).Returns(expectedClient.Orders);

        //when
        var actual = _sut.GetAllOrdersByClient(expectedClient.Id);

        //then
        var actualResult = actual.Result as ObjectResult;
        var ordersResponse = actualResult.Value as List<OrderResponse>;

        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        Assert.True(expectedClient.Orders.Count == ordersResponse.Count);
        Assert.True(expectedClient.Orders[0].Id == ordersResponse[0].Id);
        Assert.True(expectedClient.Orders[1].Price == ordersResponse[1].Price);
        Assert.True(expectedClient.Orders[0].Status == ordersResponse[0].Status);
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

        _clientsServiceMock.Setup(o => o.GetAllClients()).Returns(clients).Verifiable();

        //when
        var actual = _sut.GetAllClients();

        //then
        var actualResult = actual.Result as ObjectResult;
        var clientsResponse = actualResult.Value as List<ClientResponse>;


        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        Assert.True(clientsResponse.Count == clients.Count);
        Assert.True(clientsResponse[0].FirstName == clients[0].FirstName);
        Assert.True(clientsResponse[1].LastName == clients[1].LastName);
        Assert.True(clientsResponse[2].Email == clients[2].Email);
        Assert.True(clientsResponse[1].Phone == clients[1].Phone);
        Assert.True(clientsResponse[0].BirthDate == clients[0].BirthDate);
        _clientsServiceMock.Verify(x => x.GetAllClients(), Times.Once);
    }
}