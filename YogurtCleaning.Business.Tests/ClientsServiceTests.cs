using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Facts;

public class ClientsServiceFacts
{
    private ClientsService _sut;
    private Mock<IClientsRepository> _clientsRepositoryMock;

    private UserValues userValue;

    private void Setup()
    {
        _clientsRepositoryMock = new Mock<IClientsRepository>();
        _sut = new ClientsService(_clientsRepositoryMock.Object);
    }

    [Fact]
    public void CreateClient_WhenValidRequestPassed_ClientAdded()
    {
        //given
        Setup();
        _clientsRepositoryMock.Setup(c => c.CreateClient(It.IsAny<Client>()))
             .Returns(1);
        var expectedId = 1;

        var client = new Client()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com568",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
        };

        //when
        var actual = _sut.CreateClient(client);

        //then

        Assert.True(actual == expectedId);
        _clientsRepositoryMock.Verify(c => c.CreateClient(It.IsAny<Client>()), Times.Once);
    }

    [Fact]
    public void CreateClient_WhenNotUniqueEmail_ThrowDataException()
    {
        //given
        Setup();
        var clients = new List<Client>
        {
            new Client()
            {
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
            },
            new Client()
            {
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com1",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
                IsDeleted = true,
            },
            new Client()
            {
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com3",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
            }
        };
        var clientNew = new Client()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
        };
        _clientsRepositoryMock.Setup(c => c.GetClientByEmail(clientNew.Email))
             .Returns(clients[0]);
        //when

        //then
        Assert.Throws<Exceptions.UniquenessException>(() => _sut.CreateClient(clientNew));
    }

    [Fact]
    public void GetAllClients_WhenValidRequestPassed_ClientsReceived()
    {
        //given
        Setup();
        var clients = new List<Client>
        {
            new Client()
            {
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com1",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
            },
            new Client()
            {
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com2",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
                IsDeleted = true,
            },
            new Client()
            {
                 FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com3",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
            }
        };
        _clientsRepositoryMock.Setup(o => o.GetAllClients()).Returns(clients);
        userValue = new UserValues() {Email = "AdamSmith@gmail.com1", Role = "Admin" };

        //when
        var actual = _sut.GetAllClients();

        //then
        Assert.NotNull(actual);
        Assert.Equal(clients.Count, actual.Count);
        _clientsRepositoryMock.Verify(c => c.GetAllClients(), Times.Once);
    }

    [Fact]
    public void GetClient_WhenCurrentUserIsAdmin_ClientReceived()
    {
        //given
        Setup();
        var clientInDb = new Client()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
            BirthDate = DateTime.Today
        };

        userValue = new UserValues() { Email = "AdamSmith@gmail.com1", Role = "Admin" };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).Returns(clientInDb);

        //when
        var actual = _sut.GetClient(clientInDb.Id, userValue);

        //then
        _clientsRepositoryMock.Verify(c => c.GetClient(clientInDb.Id), Times.Once);
    }

    [Fact]
    public void GetClient_WhenIdNotInBase_GetEntityNotFoundException()
    {
        //given
        Setup();
        var testId = 2;

        var clientInDb = new Client()
        {
            Id = 1,
            FirstName = "Adam",
        };

        userValue = new UserValues() { Email = "AdamSmith@gmail.com1", Role = "Admin" };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).Returns(clientInDb);

        //when

        //then
        Assert.Throws<Exceptions.EntityNotFoundException>(() => _sut.GetClient(testId, userValue));
    }

    [Fact]
    public void GetClient_WhenClientGetSomeoneElsesProfile_ThrowAccessException()
    {
        //given
        Setup();
        var testEmail = "FakeClient@gmail.ru";
        var clientInDb = new Client()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
            BirthDate = DateTime.Today
        };
        userValue = new UserValues() { Email = "AdamSmith@gmail.com1", Role = "Client" };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).Returns(clientInDb);

        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.GetClient(clientInDb.Id, userValue));
    }

    [Fact]
    public void GetCommentsByClient_WhenClentGetOwnComments_CommentsReceived()
    {
        //given
        Setup();
        var clientInDb = new Client()
        {

            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
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
                }
        };
        userValue = new UserValues() { Email = clientInDb.Email, Role = "Client" };

        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).Returns(clientInDb);
        _clientsRepositoryMock.Setup(o => o.GetAllCommentsByClient(clientInDb.Id)).Returns(clientInDb.Comments);

        //when
        var actual = _sut.GetCommentsByClient(clientInDb.Id, userValue);

        //then

        Assert.Equal(clientInDb.Comments.Count, actual.Count);
        Assert.True(actual[0].Id == clientInDb.Comments[0].Id);
        Assert.True(actual[1].Id == clientInDb.Comments[1].Id);
        Assert.True(actual[0].Rating == clientInDb.Comments[0].Rating);
        Assert.True(actual[1].Rating == clientInDb.Comments[1].Rating);
        _clientsRepositoryMock.Verify(c => c.GetClient(clientInDb.Id), Times.Once);
        _clientsRepositoryMock.Verify(c => c.GetAllCommentsByClient(clientInDb.Id), Times.Once);
    }

    [Fact]
    public void GetCommentsByClient_WhenClientGetSomeoneElsesComments_ThrowBadRequestException()
    {
        //given
        Setup();
        var clientInDb = new Client();
        var testEmail = "FakeClient@gmail.ru";

        userValue = new UserValues() { Email = testEmail, Role = "Client" };

        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).Returns(clientInDb);
        _clientsRepositoryMock.Setup(o => o.GetAllCommentsByClient(clientInDb.Id)).Returns(clientInDb.Comments);
        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.GetCommentsByClient(clientInDb.Id, userValue));
    }

    [Fact]
    public void GetCommentsByClientId_WhenAdminGetsCommentsAndClientIsNotInDb_ThrowBadRequestException()
    {
        //given
        Setup();
        var testEmail = "FakeClient@gmail.ru";
        var clientInDb = new Client()
        {
            Id = 1,
            FirstName = "Adam",
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
            }

        };
        userValue = new UserValues() { Email = testEmail, Role = "Admin" };

        _clientsRepositoryMock.Setup(o => o.GetAllCommentsByClient(clientInDb.Id)).Returns(clientInDb.Comments);
        //when

        //then
        Assert.Throws<Exceptions.BadRequestException>(() => _sut.GetCommentsByClient(clientInDb.Id, userValue));
    }

    [Fact]
    public void GetOrdersByClientId_WhenClentGetsOwnOrders_OrdersReceived()
    {
        //given
        Setup();
        var clientInDb = new Client()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
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
            }
        };
        userValue = new UserValues() { Email = clientInDb.Email, Role = "Client" };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).Returns(clientInDb);
        _clientsRepositoryMock.Setup(o => o.GetAllOrdersByClient(clientInDb.Id)).Returns(clientInDb.Orders);

        //when
        var actual = _sut.GetOrdersByClient(clientInDb.Id, userValue);

        //then
        Assert.True(clientInDb.Orders.Count == actual.Count);
        Assert.True(actual[0].Id == clientInDb.Orders[0].Id);
        Assert.True(actual[1].Id == clientInDb.Orders[1].Id);
        Assert.True(actual[0].Price == clientInDb.Orders[0].Price);
        Assert.True(actual[1].Price == clientInDb.Orders[1].Price);
        _clientsRepositoryMock.Verify(c => c.GetClient(clientInDb.Id), Times.Once);
        _clientsRepositoryMock.Verify(c => c.GetAllOrdersByClient(clientInDb.Id), Times.Once);
    }

    [Fact]
    public void GetOrdersByClientId_WhenClientsTryToGetSomeoneElsesOrders_ThrowBadRequestException()
    {
        //given
        Setup();
        var clientInDb = new Client();
        var testEmail = "FakeClient@gmail.ru";

        userValue = new UserValues() { Email = testEmail, Role = "Client" };

        _clientsRepositoryMock.Setup(o => o.GetAllOrdersByClient(clientInDb.Id)).Returns(clientInDb.Orders);
        //when

        //then
        Assert.Throws<Exceptions.BadRequestException>(() => _sut.GetOrdersByClient(clientInDb.Id, userValue));
    }

    [Fact]
    public void GetOrdersByClientId_AdminGetsOrderssWhenClientNotInDb_ThrowBadRequestException()
    {
        //given
        Setup();
        var testEmail = "FakeClient@gmail.ru";
        var clientInDb = new Client()
        {
            Id = 1,
            FirstName = "Adam",
            Orders = new()
            {
                new()
                {
                     Id = 1
                },
            }
        };
        userValue = new UserValues() { Email = testEmail, Role = "Admin" };

        _clientsRepositoryMock.Setup(o => o.GetAllOrdersByClient(clientInDb.Id)).Returns(clientInDb.Orders);
        //when

        //then
        Assert.Throws<Exceptions.BadRequestException>(() => _sut.GetOrdersByClient(clientInDb.Id, userValue));
    }

    [Fact]
    public void UpdateClient_WhenClientUpdatesProperties_ChangesProperties()
    {
        //given
        Setup();
        var client = new Client()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
            BirthDate = DateTime.Today
        };

        Client newClientModel = new Client()
        {
            FirstName = "Someone",
            LastName = "Else",
            Phone = "+73845277",
            BirthDate = new DateTime(1996, 10, 10),
        };
        userValue = new UserValues() { Email = client.Email, Role = "Client" }; ;
        _clientsRepositoryMock.Setup(o => o.GetClient(client.Id)).Returns(client);
        _clientsRepositoryMock.Setup(o => o.UpdateClient(newClientModel));

        //when
        _sut.UpdateClient(newClientModel, client.Id, userValue);

        //then
        _clientsRepositoryMock.Verify(c => c.GetClient(client.Id), Times.Once);
        _clientsRepositoryMock.Verify(c => c.UpdateClient(It.Is<Client>(c =>
        c.FirstName == newClientModel.FirstName &&
        c.LastName == newClientModel.LastName &&
        c.BirthDate == newClientModel.BirthDate &&
        c.Phone == newClientModel.Phone &&
        c.Id == client.Id &&
        c.Email == client.Email
        )), Times.Once);
    }

    [Fact]
    public void UpdateClient_WhenEmptyClientRequest_ThrowEntityNotFoundException()
    {
        //given
        Setup();
        var client = new Client();
        var testEmail = "FakeClient@gmail.ru";

        Client newClientModel = new Client()
        {
            FirstName = "Someone",
            LastName = "Else",
            Phone = "+73845277",
            BirthDate = new DateTime(1996, 10, 10)
        };
        userValue = new UserValues() { Email = testEmail, Role = "Client" };

        _clientsRepositoryMock.Setup(o => o.UpdateClient(newClientModel));

        //when

        //then
        Assert.Throws<Exceptions.BadRequestException>(() => _sut.UpdateClient(newClientModel, client.Id, userValue));
    }

    [Fact]
    public void UpdateClient_ClientGetSomeoneElsesProfile_ThrowAccessException()
    {
        //given
        Setup();
        var testEmail = "FakeClient@gmail.ru";

        var client = new Client()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
            BirthDate = DateTime.Today
        };

        Client newClientModel = new Client()
        {
            FirstName = "Someone",
            LastName = "Else",
            Phone = "+73845277",
            BirthDate = new DateTime(1996, 10, 10),
        };
        userValue = new UserValues() { Email = testEmail, Role = "Client" };
        _clientsRepositoryMock.Setup(o => o.GetClient(client.Id)).Returns(client);
        _clientsRepositoryMock.Setup(o => o.UpdateClient(newClientModel));

        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.UpdateClient(newClientModel, client.Id, userValue));
    }

    [Fact]
    public void DeleteClient_WhenValidRequestPassed_DeleteClient()
    {
        //given
        Setup();
        var expectedClient = new Client()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
            BirthDate = DateTime.Today,
            IsDeleted = false
        };

        _clientsRepositoryMock.Setup(o => o.GetClient(expectedClient.Id)).Returns(expectedClient);
        _clientsRepositoryMock.Setup(o => o.DeleteClient(expectedClient.Id));
        userValue = new UserValues() { Email = "AdamSmith@gmail.com3", Role = "Client", Id = 1 };

        //when
        _sut.DeleteClient(expectedClient.Id, userValue);

        //then
        _clientsRepositoryMock.Verify(c => c.DeleteClient(expectedClient.Id), Times.Once);
    }

    [Fact]
    public void DeleteClient_EmptyClientRequest_ThrowEntityNotFoundException()
    {
        //given
        Setup();
        var testId = 1;
        var client = new Client();
        var testEmail = "FakeClient@gmail.ru";
        userValue = new UserValues() { Email = testEmail, Role = "Client" };
        _clientsRepositoryMock.Setup(o => o.DeleteClient(testId));

        //when

        //then
        Assert.Throws<Exceptions.BadRequestException>(() => _sut.DeleteClient(testId, userValue));
    }

    [Fact]
    public void DeleteClient_WhenClientGetSomeoneElsesProfile_ThrowAccessException()
    {
        //given
        Setup();
        var clientFirst = new Client()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
            BirthDate = DateTime.Today,
            IsDeleted = false

        };
        var clientSecond = new Client()
        {
            Id = 2,
            FirstName = "Adam2",
            LastName = "Smith2",
            Password = "123456782",
            Email = "AdamSmith@gmail.com2",
            Phone = "5559997264",
            BirthDate = DateTime.Today,
            IsDeleted = false

        };
        userValue = new UserValues() { Email = clientFirst.Email, Role = "Client" };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientSecond.Id)).Returns(clientSecond);

        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.DeleteClient(clientSecond.Id, userValue));
    }
}