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

    private List<string> _identities;

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
            Email = "AdamSmith@gmail.com",
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
    public void CreateClient_WhenIncorrectedBirthDate_ThrowDataException()
    {
        //given
        Setup();
        _clientsRepositoryMock.Setup(c => c.CreateClient(It.IsAny<Client>()))
             .Returns(1);

        var client = new Client()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            BirthDate = new DateTime(2023, 05, 05),
        };

        //when

        //then
        Assert.Throws<Exceptions.DataException>(() => _sut.CreateClient(client));
    }

    [Fact]
    public void AddClient_WhenNotUniqueEmail_ThrowDataException()
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

        _clientsRepositoryMock.Setup(c => c.GetAllClients())
             .Returns(clients);

        var clientNew = new Client()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
        };

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
        _clientsRepositoryMock.Setup(o => o.GetAllClients()).Returns(clients).Verifiable();
        _identities = new List<string>() { "AdamSmith@gmail.com1", Role.Admin.ToString() };

        //when
        var actual = _sut.GetAllClients(_identities);

        //then
        Assert.NotNull(actual);
        Assert.True(actual.GetType() == typeof(List<Client>));
        Assert.Null(actual[0].Comments);
        Assert.Null(actual[1].Orders);
        Assert.True(actual[0].IsDeleted == false);
        Assert.True(actual[1].IsDeleted == true);
        _clientsRepositoryMock.Verify(c => c.GetAllClients(), Times.Once);
    }

    [Fact]
    public void CreateClient_WhenIncorrectedPhoneNumber_ThrowDataException()
    {
        //given
        Setup();
        var client = new Client()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
            BirthDate = DateTime.Today,
        };
        _clientsRepositoryMock.Setup(c => c.CreateClient(It.IsAny<Client>()))
             .Returns(1);

        //when

        //then
        Assert.Throws<Exceptions.DataException>(() => _sut.CreateClient(client));
    }

    [Fact]
    public void GetClient_WhenValidRequestPassed_ClientReceived()
    {
        //given
        Setup();
        var clientInDb = new Client()
        {
            Id =1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
            BirthDate = DateTime.Today
        };

        _identities = new List<string>() { "AdamSmith@gmail.com1", Role.Admin.ToString() };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).Returns(clientInDb);

        //when
        var actual = _sut.GetClient(clientInDb.Id, _identities);

        //then
        Assert.True(actual.Id == clientInDb.Id);
        Assert.True(actual.FirstName == clientInDb.FirstName);
        Assert.True(actual.LastName == clientInDb.LastName);
        Assert.True(actual.Email == clientInDb.Email);
        Assert.True(actual.Password == clientInDb.Password);
        Assert.True(actual.Phone == clientInDb.Phone);
        Assert.True(actual.IsDeleted == false);
        _clientsRepositoryMock.Verify(c => c.CreateClient(It.IsAny<Client>()), Times.Never);
        _clientsRepositoryMock.Verify(c => c.DeleteClient(It.IsAny<int>()), Times.Never);
        _clientsRepositoryMock.Verify(c => c.GetClient(It.IsAny<int>()), Times.Once);
        _clientsRepositoryMock.Verify(c => c.GetAllClients(), Times.Never);
    }

    [Fact]
    public void GetClient_WhenEmptyRequest_ClientReceived()
    {
        //given
        Setup();
        var testId = 2;

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

        _identities = new List<string>() { "AdamSmith@gmail.com1", Role.Admin.ToString() };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).Returns(clientInDb);

        //when


        //then

        Assert.Throws<Exceptions.EntityNotFoundException>(() => _sut.GetClient(testId, _identities));
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
        _identities = new List<string>() { testEmail, Role.Client.ToString() };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).Returns(clientInDb);

        //when


        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.GetClient(clientInDb.Id, _identities));
    }

    [Fact]
    public void GetCommentsByClient_WhenValidRequestPassed_CommentsReceived()
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
        _identities = new List<string>() { clientInDb.Email, Role.Client.ToString() };

        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).Returns(clientInDb);
        _clientsRepositoryMock.Setup(o => o.GetAllCommentsByClient(clientInDb.Id)).Returns(clientInDb.Comments);

        //when
        var actual = _sut.GetCommentsByClient(clientInDb.Id, _identities);

        //then

        Assert.Equal(clientInDb.Comments.Count, actual.Count);
        Assert.True(actual[0].Id == clientInDb.Comments[0].Id);
        Assert.True(actual[1].Id == clientInDb.Comments[1].Id);
        Assert.True(actual[0].Rating == clientInDb.Comments[0].Rating);
        Assert.True(actual[1].Rating == clientInDb.Comments[1].Rating);
        _clientsRepositoryMock.Verify(c => c.GetClient(It.IsAny<int>()), Times.Once);
        _clientsRepositoryMock.Verify(c => c.GetAllClients(), Times.Never);
        _clientsRepositoryMock.Verify(c => c.GetAllCommentsByClient(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void GetCommentsByClient_WhenEmptyClientRequest_ThrowEntityNotFoundException()
    {
        //given
        Setup();
        var clientInDb = new Client();
        var testEmail = "FakeClient@gmail.ru";

        _identities = new List<string>() { testEmail, Role.Client.ToString() };

        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).Returns(clientInDb);
        _clientsRepositoryMock.Setup(o => o.GetAllCommentsByClient(clientInDb.Id)).Returns(clientInDb.Comments);
        //when


        //then
        Assert.Throws<Exceptions.EntityNotFoundException>(() => _sut.GetCommentsByClient(clientInDb.Id, _identities));
    }

    [Fact]
    public void GetCommentsByClientId_ClientGetSomeoneElsesProfile_ThrowAccessException()
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
        _identities = new List<string>() { testEmail, Role.Client.ToString() };

        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).Returns(clientInDb);
        _clientsRepositoryMock.Setup(o => o.GetAllCommentsByClient(clientInDb.Id)).Returns(clientInDb.Comments);
        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.GetCommentsByClient(clientInDb.Id, _identities));
    }


    [Fact]
    public void GetOrdersByClientId_ValidRequestPassed_RequestedTypeReceived()
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
        _identities = new List<string>() { clientInDb.Email, Role.Client.ToString() };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).Returns(clientInDb);
        _clientsRepositoryMock.Setup(o => o.GetAllOrdersByClient(clientInDb.Id)).Returns(clientInDb.Orders);

        //when
        var actual = _sut.GetOrdersByClient(clientInDb.Id, _identities);

        //then
        Assert.True(clientInDb.Orders.Count == actual.Count);
        Assert.True(actual[0].Id == clientInDb.Orders[0].Id);
        Assert.True(actual[1].Id == clientInDb.Orders[1].Id);
        Assert.True(actual[0].Price == clientInDb.Orders[0].Price);
        Assert.True(actual[1].Price == clientInDb.Orders[1].Price);
        _clientsRepositoryMock.Verify(c => c.GetClient(It.IsAny<int>()), Times.Once);
        _clientsRepositoryMock.Verify(c => c.GetAllOrdersByClient(It.IsAny<int>()), Times.Once);
        _clientsRepositoryMock.Verify(c => c.GetAllCommentsByClient(It.IsAny<int>()), Times.Never);
    }


    [Fact]
    public void GetOrdersByClientId_EmptyClientRequest_ThrowEntityNotFoundException()
    {
        //given
        Setup();
        var clientInDb = new Client();
        var testEmail = "FakeClient@gmail.ru";

        _identities = new List<string>() { testEmail, Role.Client.ToString() };

        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).Returns(clientInDb);
        _clientsRepositoryMock.Setup(o => o.GetAllOrdersByClient(clientInDb.Id)).Returns(clientInDb.Orders);
        //when


        //then
        Assert.Throws<Exceptions.EntityNotFoundException>(() => _sut.GetOrdersByClient(clientInDb.Id, _identities));
    }

    [Fact]
    public void GetOrdersByClient_WhenClientGetSomeoneElsesProfile_ThrowAccessException()
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
        var testEmail = "FakeClient@gmail.ru";
        _identities = new List<string>() { testEmail, Role.Client.ToString() };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientInDb.Id)).Returns(clientInDb);
        _clientsRepositoryMock.Setup(o => o.GetAllOrdersByClient(clientInDb.Id)).Returns(clientInDb.Orders);
        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.GetOrdersByClient(clientInDb.Id, _identities));
    }

    [Fact]
    public void UpdateClient_ValidRequestPassed_ChangesProperties()
    {
        //given
        Setup();
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
        _identities = new List<string>() { client.Email, Role.Client.ToString() };
        _clientsRepositoryMock.Setup(o => o.GetClient(client.Id)).Returns(client);
        _clientsRepositoryMock.Setup(o => o.UpdateClient(newClientModel));


        //when
        _sut.UpdateClient(newClientModel, client.Id, _identities);

        //then
        var actual = _sut.GetClient(client.Id, _identities);


        Assert.True(client.FirstName == actual.FirstName);
        Assert.True(client.LastName == actual.LastName);
        Assert.True(client.BirthDate == actual.BirthDate);
        _clientsRepositoryMock.Verify(c => c.GetClient(It.IsAny<int>()), Times.Exactly(2));
        _clientsRepositoryMock.Verify(c => c.UpdateClient(It.IsAny<Client>()), Times.Once);
        _clientsRepositoryMock.Verify(c => c.GetAllOrdersByClient(It.IsAny<int>()), Times.Never);
        _clientsRepositoryMock.Verify(c => c.GetAllCommentsByClient(It.IsAny<int>()), Times.Never);
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
        _identities = new List<string>() { testEmail, Role.Client.ToString() };

        _clientsRepositoryMock.Setup(o => o.UpdateClient(newClientModel));

        //when

        //then
        Assert.Throws<Exceptions.EntityNotFoundException>(() => _sut.UpdateClient(newClientModel, client.Id, _identities));
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
        _identities = new List<string>() { testEmail, Role.Client.ToString() };
        _clientsRepositoryMock.Setup(o => o.GetClient(client.Id)).Returns(client);
        _clientsRepositoryMock.Setup(o => o.UpdateClient(newClientModel));

        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.UpdateClient(newClientModel, client.Id, _identities));
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
        _identities = new List<string>() { expectedClient.Email, Role.Admin.ToString() };

        //when
        _sut.DeleteClient(expectedClient.Id, _identities);

        //then

        var allClients = _sut.GetAllClients(_identities);
        Assert.True(allClients is null);
        _clientsRepositoryMock.Verify(c => c.DeleteClient(It.IsAny<int>()), Times.Once);
        _clientsRepositoryMock.Verify(c => c.GetClient(It.IsAny<int>()), Times.Once);
        _clientsRepositoryMock.Verify(c => c.GetAllClients(), Times.Once);
        _clientsRepositoryMock.Verify(c => c.UpdateClient(It.IsAny<Client>()), Times.Never);
    }

    [Fact]
    public void DeleteClient_EmptyClientRequest_ThrowEntityNotFoundException()
    {
        //given
        Setup();
        var testId = 1;
        var client = new Client();
        var testEmail = "FakeClient@gmail.ru";
        _identities = new List<string>() { testEmail, Role.Client.ToString() };
        _clientsRepositoryMock.Setup(o => o.DeleteClient(testId));

        //when

        //then
        Assert.Throws<Exceptions.EntityNotFoundException>(() => _sut.DeleteClient(testId, _identities));
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
        _identities = new List<string>() { clientFirst.Email, Role.Client.ToString() };
        _clientsRepositoryMock.Setup(o => o.GetClient(clientSecond.Id)).Returns(clientSecond);

        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.DeleteClient(clientSecond.Id, _identities));
    }
}