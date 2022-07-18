using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Facts;

public class CleanersServiceFacts
{
    private CleanersService _sut;
    private Mock<ICleanersRepository> _cleanersRepositoryMock;

    private List<string> _identities;

    private void Setup()
    {
        _cleanersRepositoryMock = new Mock<ICleanersRepository>();
        _sut = new CleanersService(_cleanersRepositoryMock.Object);
    }

    [Fact]
    public void CreateCleaner_WhenValidRequestPassed_CleanerAdded()
    {
        //given
        Setup();
        _cleanersRepositoryMock.Setup(c => c.CreateCleaner(It.IsAny<Cleaner>()))
             .Returns(1);
        var expectedId = 1;

        var cleaner = new Cleaner()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
        };

        //when
        var actual = _sut.CreateCleaner(cleaner);

        //then

        Assert.True(actual == expectedId);
        _cleanersRepositoryMock.Verify(c => c.CreateCleaner(It.IsAny<Cleaner>()), Times.Once);
    }

    [Fact]
    public void CreateCleaner_WhenIncorrectedBirthDate_ThrowDataException()
    {
        //given
        Setup();
        _cleanersRepositoryMock.Setup(c => c.CreateCleaner(It.IsAny<Cleaner>()))
             .Returns(1);

        var cleaner = new Cleaner()
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
        Assert.Throws<Exceptions.DataException>(() => _sut.CreateCleaner(cleaner));
    }

    [Fact]
    public void AddCleaner_WhenNotUniqueEmail_ThrowDataException()
    {
        //given
        Setup();
        var cleaners = new List<Cleaner>
        {
            new Cleaner()
            {
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
            },
            new Cleaner()
            {
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com1",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
                IsDeleted = true,
            },
            new Cleaner()
            {
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com3",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
            }
        };

        _cleanersRepositoryMock.Setup(c => c.GetAllCleaners())
             .Returns(cleaners);

        var cleanerNew = new Cleaner()
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
        Assert.Throws<Exceptions.UniquenessException>(() => _sut.CreateCleaner(cleanerNew));
    }

    [Fact]
    public void GetAllCleaners_WhenValidRequestPassed_CleanersReceived()
    {
        //given
        Setup();
        var cleaners = new List<Cleaner>
        {
            new Cleaner()
            {
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com1",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
            },
            new Cleaner()
            {
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com2",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
                IsDeleted = true,
            },
            new Cleaner()
            {
                 FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com3",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
            }
        };
        _cleanersRepositoryMock.Setup(o => o.GetAllCleaners()).Returns(cleaners).Verifiable();
        _identities = new List<string>() { "AdamSmith@gmail.com1", Role.Admin.ToString() };

        //when
        var actual = _sut.GetAllCleaners(_identities);

        //then
        Assert.NotNull(actual);
        Assert.True(actual.GetType() == typeof(List<Cleaner>));
        Assert.Null(actual[0].Comments);
        Assert.Null(actual[1].Orders);
        Assert.True(actual[0].IsDeleted == false);
        Assert.True(actual[1].IsDeleted == true);
        _cleanersRepositoryMock.Verify(c => c.GetAllCleaners(), Times.Once);
    }

    [Fact]
    public void CreateCleaner_WhenIncorrectedPhoneNumber_ThrowDataException()
    {
        //given
        Setup();
        var cleaner = new Cleaner()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
            BirthDate = DateTime.Today,
        };
        _cleanersRepositoryMock.Setup(c => c.CreateCleaner(It.IsAny<Cleaner>()))
             .Returns(1);

        //when

        //then
        Assert.Throws<Exceptions.DataException>(() => _sut.CreateCleaner(cleaner));
    }

    [Fact]
    public void GetCleaner_WhenValidRequestPassed_CleanerReceived()
    {
        //given
        Setup();
        var cleanerInDb = new Cleaner()
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
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).Returns(cleanerInDb);

        //when
        var actual = _sut.GetCleaner(cleanerInDb.Id, _identities);

        //then
        Assert.True(actual.Id == cleanerInDb.Id);
        Assert.True(actual.FirstName == cleanerInDb.FirstName);
        Assert.True(actual.LastName == cleanerInDb.LastName);
        Assert.True(actual.Email == cleanerInDb.Email);
        Assert.True(actual.Password == cleanerInDb.Password);
        Assert.True(actual.Phone == cleanerInDb.Phone);
        Assert.True(actual.IsDeleted == false);
        _cleanersRepositoryMock.Verify(c => c.CreateCleaner(It.IsAny<Cleaner>()), Times.Never);
        _cleanersRepositoryMock.Verify(c => c.DeleteCleaner(It.IsAny<int>()), Times.Never);
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(It.IsAny<int>()), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.GetAllCleaners(), Times.Never);
    }

    [Fact]
    public void GetCleaner_WhenEmptyRequest_CleanerReceived()
    {
        //given
        Setup();
        var testId = 2;

        var cleanerInDb = new Cleaner()
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
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).Returns(cleanerInDb);

        //when


        //then

        Assert.Throws<Exceptions.EntityNotFoundException>(() => _sut.GetCleaner(testId, _identities));
    }

    [Fact]
    public void GetCleaner_WhenCleanerGetSomeoneElsesProfile_ThrowAccessException()
    {
        //given
        Setup();
        var testEmail = "FakeCleaner@gmail.ru";

        var cleanerInDb = new Cleaner()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
            BirthDate = DateTime.Today
        };
        _identities = new List<string>() { testEmail, Role.Cleaner.ToString() };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).Returns(cleanerInDb);

        //when


        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.GetCleaner(cleanerInDb.Id, _identities));
    }

    [Fact]
    public void GetCommentsByCleaner_WhenValidRequestPassed_CommentsReceived()
    {
        //given
        Setup();
        var cleanerInDb = new Cleaner()
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
        _identities = new List<string>() { cleanerInDb.Email, Role.Cleaner.ToString() };

        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).Returns(cleanerInDb);
        _cleanersRepositoryMock.Setup(o => o.GetAllCommentsByCleaner(cleanerInDb.Id)).Returns(cleanerInDb.Comments);

        //when
        var actual = _sut.GetCommentsByCleaner(cleanerInDb.Id, _identities);

        //then

        Assert.Equal(cleanerInDb.Comments.Count, actual.Count);
        Assert.True(actual[0].Id == cleanerInDb.Comments[0].Id);
        Assert.True(actual[1].Id == cleanerInDb.Comments[1].Id);
        Assert.True(actual[0].Rating == cleanerInDb.Comments[0].Rating);
        Assert.True(actual[1].Rating == cleanerInDb.Comments[1].Rating);
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(It.IsAny<int>()), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.GetAllCleaners(), Times.Never);
        _cleanersRepositoryMock.Verify(c => c.GetAllCommentsByCleaner(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void GetCommentsByCleaner_WhenEmptyCleanerRequest_ThrowEntityNotFoundException()
    {
        //given
        Setup();
        var cleanerInDb = new Cleaner();
        var testEmail = "FakeCleaner@gmail.ru";

        _identities = new List<string>() { testEmail, Role.Cleaner.ToString() };

        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).Returns(cleanerInDb);
        _cleanersRepositoryMock.Setup(o => o.GetAllCommentsByCleaner(cleanerInDb.Id)).Returns(cleanerInDb.Comments);
        //when


        //then
        Assert.Throws<Exceptions.EntityNotFoundException>(() => _sut.GetCommentsByCleaner(cleanerInDb.Id, _identities));
    }

    [Fact]
    public void GetCommentsByCleanerId_CleanerGetSomeoneElsesProfile_ThrowAccessException()
    {
        //given
        Setup();
        var testEmail = "FakeCleaner@gmail.ru";
        var cleanerInDb = new Cleaner()
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
        _identities = new List<string>() { testEmail, Role.Cleaner.ToString() };

        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).Returns(cleanerInDb);
        _cleanersRepositoryMock.Setup(o => o.GetAllCommentsByCleaner(cleanerInDb.Id)).Returns(cleanerInDb.Comments);
        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.GetCommentsByCleaner(cleanerInDb.Id, _identities));
    }


    [Fact]
    public void GetOrdersByCleanerId_ValidRequestPassed_RequestedTypeReceived()
    {
        //given
        Setup();
        var cleanerInDb = new Cleaner()
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
        _identities = new List<string>() { cleanerInDb.Email, Role.Cleaner.ToString() };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).Returns(cleanerInDb);
        _cleanersRepositoryMock.Setup(o => o.GetAllOrdersByCleaner(cleanerInDb.Id)).Returns(cleanerInDb.Orders);

        //when
        var actual = _sut.GetOrdersByCleaner(cleanerInDb.Id, _identities);

        //then
        Assert.True(cleanerInDb.Orders.Count == actual.Count);
        Assert.True(actual[0].Id == cleanerInDb.Orders[0].Id);
        Assert.True(actual[1].Id == cleanerInDb.Orders[1].Id);
        Assert.True(actual[0].Price == cleanerInDb.Orders[0].Price);
        Assert.True(actual[1].Price == cleanerInDb.Orders[1].Price);
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(It.IsAny<int>()), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.GetAllOrdersByCleaner(It.IsAny<int>()), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.GetAllCommentsByCleaner(It.IsAny<int>()), Times.Never);
    }


    [Fact]
    public void GetOrdersByCleanerId_EmptyCleanerRequest_ThrowEntityNotFoundException()
    {
        //given
        Setup();
        var cleanerInDb = new Cleaner();
        var testEmail = "FakeCleaner@gmail.ru";

        _identities = new List<string>() { testEmail, Role.Cleaner.ToString() };

        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).Returns(cleanerInDb);
        _cleanersRepositoryMock.Setup(o => o.GetAllOrdersByCleaner(cleanerInDb.Id)).Returns(cleanerInDb.Orders);
        //when


        //then
        Assert.Throws<Exceptions.EntityNotFoundException>(() => _sut.GetOrdersByCleaner(cleanerInDb.Id, _identities));
    }

    [Fact]
    public void GetOrdersByCleaner_WhenCleanerGetSomeoneElsesProfile_ThrowAccessException()
    {
        //given
        Setup();
        var cleanerInDb = new Cleaner()
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
        var testEmail = "FakeCleaner@gmail.ru";
        _identities = new List<string>() { testEmail, Role.Cleaner.ToString() };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).Returns(cleanerInDb);
        _cleanersRepositoryMock.Setup(o => o.GetAllOrdersByCleaner(cleanerInDb.Id)).Returns(cleanerInDb.Orders);
        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.GetOrdersByCleaner(cleanerInDb.Id, _identities));
    }

    [Fact]
    public void UpdateCleaner_ValidRequestPassed_ChangesProperties()
    {
        //given
        Setup();
        var cleaner = new Cleaner()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
            BirthDate = DateTime.Today
        };

        Cleaner newCleanerModel = new Cleaner()
        {
            FirstName = "Someone",
            LastName = "Else",
            Phone = "+73845277",
            BirthDate = new DateTime(1996, 10, 10),
        };
        _identities = new List<string>() { cleaner.Email, Role.Cleaner.ToString() };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleaner.Id)).Returns(cleaner);
        _cleanersRepositoryMock.Setup(o => o.UpdateCleaner(newCleanerModel));


        //when
        _sut.UpdateCleaner(newCleanerModel, cleaner.Id, _identities);

        //then
        var actual = _sut.GetCleaner(cleaner.Id, _identities);


        Assert.True(cleaner.FirstName == actual.FirstName);
        Assert.True(cleaner.LastName == actual.LastName);
        Assert.True(cleaner.BirthDate == actual.BirthDate);
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(It.IsAny<int>()), Times.Exactly(2));
        _cleanersRepositoryMock.Verify(c => c.UpdateCleaner(It.IsAny<Cleaner>()), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.GetAllOrdersByCleaner(It.IsAny<int>()), Times.Never);
        _cleanersRepositoryMock.Verify(c => c.GetAllCommentsByCleaner(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void UpdateCleaner_WhenEmptyCleanerRequest_ThrowEntityNotFoundException()
    {
        //given
        Setup();
        var cleaner = new Cleaner();
        var testEmail = "FakeCleaner@gmail.ru";

        Cleaner newCleanerModel = new Cleaner()
        {
            FirstName = "Someone",
            LastName = "Else",
            Phone = "+73845277",
            BirthDate = new DateTime(1996, 10, 10)
        };
        _identities = new List<string>() { testEmail, Role.Cleaner.ToString() };

        _cleanersRepositoryMock.Setup(o => o.UpdateCleaner(newCleanerModel));

        //when

        //then
        Assert.Throws<Exceptions.EntityNotFoundException>(() => _sut.UpdateCleaner(newCleanerModel, cleaner.Id, _identities));
    }

    [Fact]
    public void UpdateCleaner_CleanerGetSomeoneElsesProfile_ThrowAccessException()
    {
        //given
        Setup();
        var testEmail = "FakeCleaner@gmail.ru";

        var cleaner = new Cleaner()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
            BirthDate = DateTime.Today
        };


        Cleaner newCleanerModel = new Cleaner()
        {
            FirstName = "Someone",
            LastName = "Else",
            Phone = "+73845277",
            BirthDate = new DateTime(1996, 10, 10),
        };
        _identities = new List<string>() { testEmail, Role.Cleaner.ToString() };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleaner.Id)).Returns(cleaner);
        _cleanersRepositoryMock.Setup(o => o.UpdateCleaner(newCleanerModel));

        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.UpdateCleaner(newCleanerModel, cleaner.Id, _identities));
    }

    [Fact]
    public void DeleteCleaner_WhenValidRequestPassed_DeleteCleaner()
    {
        //given
        Setup();
        var expectedCleaner = new Cleaner()
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

        _cleanersRepositoryMock.Setup(o => o.GetCleaner(expectedCleaner.Id)).Returns(expectedCleaner);
        _cleanersRepositoryMock.Setup(o => o.DeleteCleaner(expectedCleaner.Id));
        _identities = new List<string>() { expectedCleaner.Email, Role.Admin.ToString() };

        //when
        _sut.DeleteCleaner(expectedCleaner.Id, _identities);

        //then

        var allCleaners = _sut.GetAllCleaners(_identities);
        Assert.True(allCleaners is null);
        _cleanersRepositoryMock.Verify(c => c.DeleteCleaner(It.IsAny<int>()), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(It.IsAny<int>()), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.GetAllCleaners(), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.UpdateCleaner(It.IsAny<Cleaner>()), Times.Never);
    }

    [Fact]
    public void DeleteCleaner_EmptyCleanerRequest_ThrowEntityNotFoundException()
    {
        //given
        Setup();
        var testId = 1;
        var cleaner = new Cleaner();
        var testEmail = "FakeCleaner@gmail.ru";
        _identities = new List<string>() { testEmail, Role.Cleaner.ToString() };
        _cleanersRepositoryMock.Setup(o => o.DeleteCleaner(testId));

        //when

        //then
        Assert.Throws<Exceptions.EntityNotFoundException>(() => _sut.DeleteCleaner(testId, _identities));
    }

    [Fact]
    public void DeleteCleaner_WhenCleanerGetSomeoneElsesProfile_ThrowAccessException()
    {
        //given
        Setup();
        var cleanerFirst = new Cleaner()
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
        var cleanerSecond = new Cleaner()
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
        _identities = new List<string>() { cleanerFirst.Email, Role.Cleaner.ToString() };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerSecond.Id)).Returns(cleanerSecond);

        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.DeleteCleaner(cleanerSecond.Id, _identities));
    }
}