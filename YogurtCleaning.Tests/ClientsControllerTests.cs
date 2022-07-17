using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.Controllers;
using YogurtCleaning.Models;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Tests;
public class ClientsControllerTests
{
    private ClientsController _sut;
    private Mock<IClientsService> ClientsServiceMock;
    private Mock<IMapper> _mapper;

    private List<string> _identities;

    [SetUp]
    public void Setup()
    {
        _mapper = new Mock<IMapper>();
        ClientsServiceMock = new Mock<IClientsService>();
        _sut = new ClientsController(_mapper.Object, ClientsServiceMock.Object);
    }

    [Test]
    public async Task CreateClient_WhenValidRequestPassed_CreatedResultReceived()
    {
        //given
        ClientsServiceMock.Setup(c => c.CreateClient(It.IsAny<Client>()))
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

        ClientsServiceMock.Verify(c => c.CreateClient(It.IsAny<Client>()), Times.Once);
        ClientsServiceMock.Verify(c => c.DeleteClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetAllClients(It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.UpdateClient(It.IsAny<Client>(), It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetOrdersByClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetCommentsByClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
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
        ClientsServiceMock.Setup(o => o.GetClient(expectedClient.Id, _identities)).Returns(expectedClient);

        //when
        var actual = _sut.GetClient(expectedClient.Id);

        //then
        var actualResult = actual.Result as ObjectResult;


        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        ClientsServiceMock.Verify(c => c.CreateClient(It.IsAny<Client>()), Times.Never);
        ClientsServiceMock.Verify(c => c.DeleteClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Once);
        ClientsServiceMock.Verify(c => c.GetAllClients(It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.UpdateClient(It.IsAny<Client>(), It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetOrdersByClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetCommentsByClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
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

        ClientsServiceMock.Setup(o => o.UpdateClient(client, client.Id, _identities));

        //when
        var actual = _sut.UpdateClient(newClientModel, client.Id);

        //then
        var actualResult = actual as NoContentResult;

        Assert.AreEqual(StatusCodes.Status204NoContent, actualResult.StatusCode);
        ClientsServiceMock.Verify(c => c.CreateClient(It.IsAny<Client>()), Times.Never);
        ClientsServiceMock.Verify(c => c.DeleteClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetAllClients(It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.UpdateClient(It.IsAny<Client>(), It.IsAny<int>(), It.IsAny<List<string>>()), Times.Once);
        ClientsServiceMock.Verify(c => c.GetOrdersByClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetCommentsByClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
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
        ClientsServiceMock.Setup(o => o.GetCommentsByClient(expectedClient.Id, _identities)).Returns(expectedClient.Comments);

        //when
        var actual = _sut.GetAllCommentsByClient(expectedClient.Id);

        //then
        var actualResult = actual.Result as ObjectResult;

        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        ClientsServiceMock.Verify(c => c.CreateClient(It.IsAny<Client>()), Times.Never);
        ClientsServiceMock.Verify(c => c.DeleteClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetAllClients(It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.UpdateClient(It.IsAny<Client>(), It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetOrdersByClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetCommentsByClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Once);
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

        ClientsServiceMock.Setup(o => o.GetOrdersByClient(expectedClient.Id, _identities)).Returns(expectedClient.Orders);

        //when
        var actual = _sut.GetAllOrdersByClient(expectedClient.Id);

        //then
        var actualResult = actual.Result as ObjectResult;

        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        ClientsServiceMock.Verify(c => c.CreateClient(It.IsAny<Client>()), Times.Never);
        ClientsServiceMock.Verify(c => c.DeleteClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetAllClients(It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.UpdateClient(It.IsAny<Client>(), It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetOrdersByClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Once);
        ClientsServiceMock.Verify(c => c.GetCommentsByClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
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

        ClientsServiceMock.Setup(o => o.GetClient(expectedClient.Id, _identities)).Returns(expectedClient);

        //when
        var actual = _sut.DeleteClient(expectedClient.Id);

        //then
        var actualResult = actual as NoContentResult;

        Assert.AreEqual(StatusCodes.Status204NoContent, actualResult.StatusCode);
        ClientsServiceMock.Verify(c => c.CreateClient(It.IsAny<Client>()), Times.Never);
        ClientsServiceMock.Verify(c => c.DeleteClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Once);
        ClientsServiceMock.Verify(c => c.GetClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetAllClients(It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.UpdateClient(It.IsAny<Client>(), It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetOrdersByClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetCommentsByClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
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

        ClientsServiceMock.Setup(o => o.GetAllClients(_identities)).Returns(clients).Verifiable();

        //when
        var actual = _sut.GetAllClients();

        //then
        var actualResult = actual.Result as ObjectResult;

        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        ClientsServiceMock.Verify(c => c.CreateClient(It.IsAny<Client>()), Times.Never);
        ClientsServiceMock.Verify(c => c.DeleteClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetAllClients(It.IsAny<List<string>>()), Times.Once);
        ClientsServiceMock.Verify(c => c.UpdateClient(It.IsAny<Client>(), It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetOrdersByClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        ClientsServiceMock.Verify(c => c.GetCommentsByClient(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
    }
}