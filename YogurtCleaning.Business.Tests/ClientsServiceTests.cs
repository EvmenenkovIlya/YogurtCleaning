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

    public ClientsServiceFacts()
    {
        _clientsRepositoryMock = new Mock<IClientsRepository>();
        _sut = new ClientsService(_clientsRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateClient_WhenValidRequestPassed_ClientAdded()
    {
        //given
        _clientsRepositoryMock.Setup(c => c.CreateClient(It.IsAny<Client>()))
             .ReturnsAsync(1);
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
        var actual = await _sut.CreateClient(client);

        //then

        Assert.Equal(expectedId, actual);
        _clientsRepositoryMock.Verify(c => c.CreateClient(It.IsAny<Client>()), Times.Once);
    }

    [Fact]
    public async Task CreateClient_WhenNotUniqueEmail_ThrowDataException()
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
             .ReturnsAsync(clients[0]);
        //when

        //then
        await Assert.ThrowsAsync<Exceptions.UniquenessException>(() => _sut.CreateClient(clientNew));
    }

    [Fact]
    public async Task GetAllClients_WhenValidRequestPassed_ClientsReceived()
    {
        //given
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
        _clientsRepositoryMock.Setup(o => o.GetAllClients()).ReturnsAsync(clients);
        userValue = new UserValues() {Email = "AdamSmith@gmail.com1", Role = Role.Admin };

        //when
        var actual = await _sut.GetAllClients();

        //then
        Assert.NotNull(actual);
        Assert.Equal(clients.Count, actual.Count);
        _clientsRepositoryMock.Verify(c => c.GetAllClients(), Times.Once);
    }

    [Fact]
    public async Task GetClient_WhenCurrentUserIsAdmin_ClientReceived()
    {
        //given
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

        userValue = new UserValues() { Email = "AdamSmith@gmail.com1", Role = Role.Admin };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).ReturnsAsync(clientInDb);

        //when
        var actual = await _sut.GetClient(clientInDb.Id, userValue);

        //then
        _clientsRepositoryMock.Verify(c => c.GetClient(clientInDb.Id), Times.Once);
    }

    [Fact]
    public async Task GetClient_WhenIdNotInBase_GetEntityNotFoundException()
    {
        //given
        var testId = 2;

        var clientInDb = new Client()
        {
            Id = 1,
            FirstName = "Adam",
        };

        userValue = new UserValues() { Email = "AdamSmith@gmail.com1", Role = Role.Admin };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).ReturnsAsync(clientInDb);

        //when

        //then
        await Assert.ThrowsAsync<Exceptions.EntityNotFoundException>(() => _sut.GetClient(testId, userValue));
    }

    [Fact]
    public async Task GetClient_WhenClientGetSomeoneElsesProfile_ThrowAccessException()
    {
        //given
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
        userValue = new UserValues() { Email = "AdamSmith@gmail.com1", Role = Role.Client };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).ReturnsAsync(clientInDb);

        //when

        //then
        await Assert.ThrowsAsync<Exceptions.AccessException>(() => _sut.GetClient(clientInDb.Id, userValue));
    }

    [Fact]
    public async Task GetCommentsByClient_WhenClentGetOwnComments_CommentsReceived()
    {
        //given
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
        userValue = new UserValues() { Email = clientInDb.Email, Role = Role.Client };

        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).ReturnsAsync(clientInDb);
        _clientsRepositoryMock.Setup(o => o.GetAllCommentsByClient(clientInDb.Id)).ReturnsAsync(clientInDb.Comments);

        //when
        var actual = await _sut.GetCommentsByClient(clientInDb.Id, userValue);

        //then

        Assert.Equal(clientInDb.Comments.Count, actual.Count);
        Assert.Equal(clientInDb.Comments[0].Id, actual[0].Id);
        Assert.Equal(clientInDb.Comments[1].Id, actual[1].Id);
        Assert.Equal(clientInDb.Comments[0].Rating, actual[0].Rating);
        Assert.Equal(clientInDb.Comments[1].Rating, actual[1].Rating);
        _clientsRepositoryMock.Verify(c => c.GetClient(clientInDb.Id), Times.Once);
        _clientsRepositoryMock.Verify(c => c.GetAllCommentsByClient(clientInDb.Id), Times.Once);
    }

    [Fact]
    public async Task GetCommentsByClient_WhenClientGetSomeoneElsesComments_ThrowBadRequestException()
    {
        //given
        var clientInDb = new Client();
        var testEmail = "FakeClient@gmail.ru";

        userValue = new UserValues() { Email = testEmail, Role = Role.Client };

        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).ReturnsAsync(clientInDb);
        _clientsRepositoryMock.Setup(o => o.GetAllCommentsByClient(clientInDb.Id)).ReturnsAsync(clientInDb.Comments);
        //when

        //then
        await Assert.ThrowsAsync<Exceptions.AccessException>(() => _sut.GetCommentsByClient(clientInDb.Id, userValue));
    }

    [Fact]
    public async Task GetCommentsByClientId_WhenAdminGetsCommentsAndClientIsNotInDb_ThrowBadRequestException()
    {
        //given
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
        userValue = new UserValues() { Email = testEmail, Role = Role.Admin };

        _clientsRepositoryMock.Setup(o => o.GetAllCommentsByClient(clientInDb.Id)).ReturnsAsync(clientInDb.Comments);
        //when

        //then
        await Assert.ThrowsAsync<Exceptions.BadRequestException>(() => _sut.GetCommentsByClient(clientInDb.Id, userValue));
    }

    [Fact]
    public async Task GetOrdersByClientId_WhenClentGetsOwnOrders_OrdersReceived()
    {
        //given
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
        userValue = new UserValues() { Email = clientInDb.Email, Role = Role.Client };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).ReturnsAsync(clientInDb);
        _clientsRepositoryMock.Setup(o => o.GetAllOrdersByClient(clientInDb.Id)).ReturnsAsync(clientInDb.Orders);

        //when
        var actual = await _sut.GetOrdersByClient(clientInDb.Id, userValue);

        //then
        Assert.Equal(clientInDb.Orders.Count, actual.Count);
        Assert.Equal(clientInDb.Orders[0].Id, actual[0].Id);
        Assert.Equal(clientInDb.Orders[1].Id, actual[1].Id);
        Assert.Equal(clientInDb.Orders[0].Price, actual[0].Price);
        Assert.Equal(clientInDb.Orders[1].Price, actual[1].Price);
        _clientsRepositoryMock.Verify(c => c.GetClient(clientInDb.Id), Times.Once);
        _clientsRepositoryMock.Verify(c => c.GetAllOrdersByClient(clientInDb.Id), Times.Once);
    }

    [Fact]
    public async Task GetOrdersByClientId_WhenClientsTryToGetSomeoneElsesOrders_ThrowBadRequestException()
    {
        //given
        var clientInDb = new Client();
        var testEmail = "FakeClient@gmail.ru";

        userValue = new UserValues() { Email = testEmail, Role = Role.Client };

        _clientsRepositoryMock.Setup(o => o.GetAllOrdersByClient(clientInDb.Id)).ReturnsAsync(clientInDb.Orders);
        //when

        //then
        await Assert.ThrowsAsync<Exceptions.BadRequestException>(() => _sut.GetOrdersByClient(clientInDb.Id, userValue));
    }

    [Fact]
    public async Task GetOrdersByClientId_AdminGetsOrderssWhenClientNotInDb_ThrowBadRequestException()
    {
        //given
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
        userValue = new UserValues() { Email = testEmail, Role = Role.Admin };

        _clientsRepositoryMock.Setup(o => o.GetAllOrdersByClient(clientInDb.Id)).ReturnsAsync(clientInDb.Orders);
        //when

        //then
        await Assert.ThrowsAsync<Exceptions.BadRequestException>(() => _sut.GetOrdersByClient(clientInDb.Id, userValue));
    }

    [Fact]
    public async Task UpdateClient_WhenClientUpdatesProperties_ChangesProperties()
    {
        //given
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
        userValue = new UserValues() { Email = client.Email, Role = Role.Client }; ;
        _clientsRepositoryMock.Setup(o => o.GetClient(client.Id)).ReturnsAsync(client);
        _clientsRepositoryMock.Setup(o => o.UpdateClient(newClientModel));

        //when
        await _sut.UpdateClient(newClientModel, client.Id, userValue);

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
    public async Task UpdateClient_WhenEmptyClientRequest_ThrowEntityNotFoundException()
    {
        //given
        var client = new Client();
        var testEmail = "FakeClient@gmail.ru";

        Client newClientModel = new Client()
        {
            FirstName = "Someone",
            LastName = "Else",
            Phone = "+73845277",
            BirthDate = new DateTime(1996, 10, 10)
        };
        userValue = new UserValues() { Email = testEmail, Role = Role.Client };

        _clientsRepositoryMock.Setup(o => o.UpdateClient(newClientModel));

        //when

        //then
        await Assert.ThrowsAsync<Exceptions.BadRequestException>(() => _sut.UpdateClient(newClientModel, client.Id, userValue));
    }

    [Fact]
    public async Task UpdateClient_ClientGetSomeoneElsesProfile_ThrowAccessException()
    {
        //given
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
        userValue = new UserValues() { Email = testEmail, Role = Role.Client };
        _clientsRepositoryMock.Setup(o => o.GetClient(client.Id)).ReturnsAsync(client);
        _clientsRepositoryMock.Setup(o => o.UpdateClient(newClientModel));

        //when

        //then
        await Assert.ThrowsAsync<Exceptions.AccessException>(() => _sut.UpdateClient(newClientModel, client.Id, userValue));
    }

    [Fact]
    public async Task DeleteClient_WhenValidRequestPassed_DeleteClient()
    {
        //given
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

        _clientsRepositoryMock.Setup(o => o.GetClient(expectedClient.Id)).ReturnsAsync(expectedClient);
        _clientsRepositoryMock.Setup(o => o.DeleteClient(expectedClient));
        userValue = new UserValues() { Email = "AdamSmith@gmail.com3", Role = Role.Client, Id = 1 };

        //when
        _sut.DeleteClient(expectedClient.Id, userValue);

        //then
        _clientsRepositoryMock.Verify(c => c.DeleteClient(expectedClient), Times.Once);
    }

    [Fact]
    public async Task DeleteClient_EmptyClientRequest_ThrowEntityNotFoundException()
    {
        //given
        var testId = 1;
        var client = new Client();
        var testEmail = "FakeClient@gmail.ru";
        userValue = new UserValues() { Email = testEmail, Role = Role.Client };

        //when

        //then
        await Assert.ThrowsAsync<Exceptions.BadRequestException>(() => _sut.DeleteClient(testId, userValue));
    }

    [Fact]
    public async Task DeleteClient_WhenClientGetSomeoneElsesProfile_ThrowAccessException()
    {
        //given
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
        userValue = new UserValues() { Email = clientFirst.Email, Role = Role.Client };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientSecond.Id)).ReturnsAsync(clientSecond);

        //when

        //then
        await Assert.ThrowsAsync<Exceptions.AccessException>(() => _sut.DeleteClient(clientSecond.Id, userValue));
    }
}