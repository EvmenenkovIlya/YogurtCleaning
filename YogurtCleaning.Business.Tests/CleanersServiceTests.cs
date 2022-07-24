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
    private Mock<IOrdersRepository> _ordersRepositoryMock;

    private UserValues userValue;

    private void Setup()
    {
        _cleanersRepositoryMock = new Mock<ICleanersRepository>();
        _ordersRepositoryMock = new Mock<IOrdersRepository>();
        _sut = new CleanersService(_cleanersRepositoryMock.Object, _ordersRepositoryMock.Object);
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
            Email = "AdamSmith@gmail.com568",
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
    public void CreateCleaner_WhenNotUniqueEmail_ThrowUniquenessException()
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
        var cleanerNew = new Cleaner()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
        };
        _cleanersRepositoryMock.Setup(c => c.GetCleanerByEmail(cleanerNew.Email))
             .Returns(cleaners[0]);
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
        _cleanersRepositoryMock.Setup(o => o.GetAllCleaners()).Returns(cleaners);
        userValue = new UserValues() { Email = "AdamSmith@gmail.com1", Role = "Admin" };

        //when
        var actual = _sut.GetAllCleaners();

        //then
        Assert.NotNull(actual);
        Assert.Equal(cleaners.Count, actual.Count);
        _cleanersRepositoryMock.Verify(c => c.GetAllCleaners(), Times.Once);
    }

    [Fact]
    public void GetCleaner_WhenCurrentUserIsAdmin_CleanerReceived()
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

        userValue = new UserValues() { Email = "AdamSmith@gmail.com1", Role = "Admin" };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).Returns(cleanerInDb);

        //when
        var actual = _sut.GetCleaner(cleanerInDb.Id, userValue);

        //then
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(cleanerInDb.Id), Times.Once);
    }

    [Fact]
    public void GetCleaner_WhenIdNotInBase_GetEntityNotFoundException()
    {
        //given
        Setup();
        var testId = 2;

        var cleanerInDb = new Cleaner()
        {
            Id = 1,
            FirstName = "Adam",
        };

        userValue = new UserValues() { Email = "AdamSmith@gmail.com1", Role = "Admin" };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).Returns(cleanerInDb);

        //when

        //then
        Assert.Throws<Exceptions.EntityNotFoundException>(() => _sut.GetCleaner(testId, userValue));
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
        userValue = new UserValues() { Email = "AdamSmith@gmail.com1", Role = "Cleaner" };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).Returns(cleanerInDb);

        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.GetCleaner(cleanerInDb.Id, userValue));
    }

    [Fact]
    public void GetCommentsByCleaner_WhenClentGetOwnComments_CommentsReceived()
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
        userValue = new UserValues() { Email = cleanerInDb.Email, Role = "Cleaner" };

        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).Returns(cleanerInDb);
        _cleanersRepositoryMock.Setup(o => o.GetAllCommentsByCleaner(cleanerInDb.Id)).Returns(cleanerInDb.Comments);

        //when
        var actual = _sut.GetCommentsByCleaner(cleanerInDb.Id, userValue);

        //then

        Assert.Equal(cleanerInDb.Comments.Count, actual.Count);
        Assert.True(actual[0].Id == cleanerInDb.Comments[0].Id);
        Assert.True(actual[1].Id == cleanerInDb.Comments[1].Id);
        Assert.True(actual[0].Rating == cleanerInDb.Comments[0].Rating);
        Assert.True(actual[1].Rating == cleanerInDb.Comments[1].Rating);
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(cleanerInDb.Id), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.GetAllCommentsByCleaner(cleanerInDb.Id), Times.Once);
    }

    [Fact]
    public void GetCommentsByCleaner_WhenCleanerGetSomeoneElsesComments_ThrowBadRequestException()
    {
        //given
        Setup();
        var cleanerInDb = new Cleaner();
        var testEmail = "FakeCleaner@gmail.ru";

        userValue = new UserValues() { Email = testEmail, Role = "Cleaner" };

        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).Returns(cleanerInDb);
        _cleanersRepositoryMock.Setup(o => o.GetAllCommentsByCleaner(cleanerInDb.Id)).Returns(cleanerInDb.Comments);
        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.GetCommentsByCleaner(cleanerInDb.Id, userValue));
    }

    [Fact]
    public void GetCommentsByCleanerId_AdminGetsCommentsWhenCleanerNotInDb_ThrowBadRequestException()
    {
        //given
        Setup();
        var testEmail = "FakeCleaner@gmail.ru";
        var cleanerInDb = new Cleaner()
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

        _cleanersRepositoryMock.Setup(o => o.GetAllCommentsByCleaner(cleanerInDb.Id)).Returns(cleanerInDb.Comments);
        //when

        //then
        Assert.Throws<Exceptions.BadRequestException>(() => _sut.GetCommentsByCleaner(cleanerInDb.Id, userValue));
    }

    [Fact]
    public void GetOrdersByCleanerId_WhenClentGetsOwnOrders_OrdersReceived()
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
        userValue = new UserValues() { Email = cleanerInDb.Email, Role = "Cleaner" };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).Returns(cleanerInDb);
        _cleanersRepositoryMock.Setup(o => o.GetAllOrdersByCleaner(cleanerInDb.Id)).Returns(cleanerInDb.Orders);

        //when
        var actual = _sut.GetOrdersByCleaner(cleanerInDb.Id, userValue);

        //then
        Assert.True(cleanerInDb.Orders.Count == actual.Count);
        Assert.True(actual[0].Id == cleanerInDb.Orders[0].Id);
        Assert.True(actual[1].Id == cleanerInDb.Orders[1].Id);
        Assert.True(actual[0].Price == cleanerInDb.Orders[0].Price);
        Assert.True(actual[1].Price == cleanerInDb.Orders[1].Price);
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(cleanerInDb.Id), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.GetAllOrdersByCleaner(cleanerInDb.Id), Times.Once);
    }

    [Fact]
    public void GetOrdersByCleanerId_WhenCleanersTryToGetSomeoneElsesOrders_ThrowBadRequestException()
    {
        //given
        Setup();
        var cleanerInDb = new Cleaner();
        var testEmail = "FakeCleaner@gmail.ru";

        userValue = new UserValues() { Email = testEmail, Role = "Cleaner" };

        _cleanersRepositoryMock.Setup(o => o.GetAllOrdersByCleaner(cleanerInDb.Id)).Returns(cleanerInDb.Orders);
        //when

        //then
        Assert.Throws<Exceptions.BadRequestException>(() => _sut.GetOrdersByCleaner(cleanerInDb.Id, userValue));
    }

    [Fact]
    public void GetCommentsByCleanerId_AdminGetsOrderssWhenCleanerNotInDb_ThrowBadRequestException()
    {
        //given
        Setup();
        var testEmail = "FakeCleaner@gmail.ru";
        var cleanerInDb = new Cleaner()
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

        _cleanersRepositoryMock.Setup(o => o.GetAllOrdersByCleaner(cleanerInDb.Id)).Returns(cleanerInDb.Orders);
        //when

        //then
        Assert.Throws<Exceptions.BadRequestException>(() => _sut.GetOrdersByCleaner(cleanerInDb.Id, userValue));
    }

    [Fact]
    public void UpdateCleaner_WhenCleanerUpdatesProperties_ChangesProperties()
    {
        //given
        Setup();
        var cleaner = new Cleaner()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
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
        userValue = new UserValues() { Email = cleaner.Email, Role = "Cleaner" }; ;
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleaner.Id)).Returns(cleaner);
        _cleanersRepositoryMock.Setup(o => o.UpdateCleaner(newCleanerModel));

        //when
        _sut.UpdateCleaner(newCleanerModel, cleaner.Id, userValue);

        //then
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(cleaner.Id), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.UpdateCleaner(It.Is<Cleaner>(c =>
        c.FirstName == newCleanerModel.FirstName && c.LastName == newCleanerModel.LastName &&
        c.BirthDate == newCleanerModel.BirthDate && c.Phone == newCleanerModel.Phone &&
        c.Id == cleaner.Id && c.Email == cleaner.Email
        )), Times.Once);
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
        userValue = new UserValues() { Email = testEmail, Role = "Cleaner" };

        _cleanersRepositoryMock.Setup(o => o.UpdateCleaner(newCleanerModel));

        //when

        //then
        Assert.Throws<Exceptions.BadRequestException>(() => _sut.UpdateCleaner(newCleanerModel, cleaner.Id, userValue));
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
        userValue = new UserValues() { Email = testEmail, Role = "Cleaner" };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleaner.Id)).Returns(cleaner);
        _cleanersRepositoryMock.Setup(o => o.UpdateCleaner(newCleanerModel));

        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.UpdateCleaner(newCleanerModel, cleaner.Id, userValue));
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
        userValue = new UserValues() { Email = "AdamSmith@gmail.com3", Role = "Cleaner", Id = 1 };

        //when
        _sut.DeleteCleaner(expectedCleaner.Id, userValue);

        //then
        _cleanersRepositoryMock.Verify(c => c.DeleteCleaner(expectedCleaner.Id), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public void DeleteCleaner_EmptyCleanerRequest_ThrowEntityNotFoundException()
    {
        //given
        Setup();
        var testId = 1;
        var cleaner = new Cleaner();
        var testEmail = "FakeCleaner@gmail.ru";
        userValue = new UserValues() { Email = testEmail, Role = "Cleaner" };
        _cleanersRepositoryMock.Setup(o => o.DeleteCleaner(testId));

        //when

        //then
        Assert.Throws<Exceptions.BadRequestException>(() => _sut.DeleteCleaner(testId, userValue));
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
        userValue = new UserValues() { Email = cleanerFirst.Email, Role = "Cleaner" };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerSecond.Id)).Returns(cleanerSecond);

        //when

        //then
        Assert.Throws<Exceptions.AccessException>(() => _sut.DeleteCleaner(cleanerSecond.Id, userValue));
    }

    [Fact]
    public void GetFreeCleanersForOrder_WhenAreFreeCleaners_ThenCleanersRecieved()
    {
        //given
        Setup();
        var order = new Order
        {
            Client = new() { Id = 1 },
            CleaningObject = new() { Id = 56 },
            Status = Status.Created,
            StartTime = new DateTime(2022, 8, 1, 10, 00, 00),
            EndTime = new DateTime(2022, 8, 1, 18, 00, 00),
            Bundles = new List<Bundle> { new Bundle { Id = 2, Name = "qwe" } }
        };

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
                Schedule = Schedule.FullTime,
                Orders = new List<Order>(),
                DateOfStartWork = new DateTime(2022, 8, 1, 00, 00, 00)
            },
            new Cleaner()
            {
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com2",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
                Schedule = Schedule.FullTime,
                Orders = new List<Order>(),
                DateOfStartWork = new DateTime(2022, 8, 1, 00, 00, 00)
            },
            new Cleaner()
            {
                 FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com3",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
                Schedule = Schedule.ShiftWork,
                Orders = new List<Order>(),
                DateOfStartWork = new DateTime(2022, 8, 1, 00, 00, 00)
            }
        };
        _cleanersRepositoryMock.Setup(o => o.GetAllCleaners()).Returns(cleaners);

        //when
        var actual = _sut.GetFreeCleanersForOrder(order);

        //then
        Assert.NotNull(actual);
        Assert.Equal(cleaners.Count, actual.Count);
        _cleanersRepositoryMock.Verify(c => c.GetAllCleaners(), Times.Once);
    }
    [Fact]
    public void GetFreeCleanersForOrder_WhenCleanerIsNotWorking_ThenDoNotAddToResult()
    {
        //given
        Setup();
        var order = new Order
        {
            Client = new() { Id = 1 },
            CleaningObject = new() { Id = 56 },
            Status = Status.Created,
            StartTime = new DateTime(2022, 8, 6, 10, 00, 00),
            EndTime = new DateTime(2022, 8, 6, 18, 00, 00),
            Bundles = new List<Bundle> { new Bundle { Id = 2, Name = "qwe" } }
        };

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
                Schedule = Schedule.ShiftWork,
                DateOfStartWork = new DateTime(2022, 8, 4, 00, 00, 00)
            },
            new Cleaner()
            {
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com2",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
                Schedule = Schedule.FullTime,
                Orders = new List<Order>(),
                DateOfStartWork = new DateTime(2022, 8, 1, 00, 00, 00)
            },
            new Cleaner()
            {
                 FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com3",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
                Schedule = Schedule.ShiftWork,
                Orders = new List<Order>(),
                DateOfStartWork = new DateTime(2022, 8, 5, 00, 00, 00)
            }
        };
        _cleanersRepositoryMock.Setup(o => o.GetAllCleaners()).Returns(cleaners);
        var expectedCount = 1;

        //when
        var actual = _sut.GetFreeCleanersForOrder(order);

        //then
        Assert.NotNull(actual);
        Assert.Equal(expectedCount, actual.Count);
        _cleanersRepositoryMock.Verify(c => c.GetAllCleaners(), Times.Once);
    }

    public void GetFreeCleanersForOrder_WhenCleanerIsBusy_ThenDoNotAddToResult()
    {
        //given
        Setup();
        var order = new Order
        {
            Client = new() { Id = 1 },
            CleaningObject = new() { Id = 56 },
            Status = Status.Created,
            StartTime = new DateTime(2022, 8, 1, 10, 00, 00),
            EndTime = new DateTime(2022, 8, 1, 18, 00, 00),
            Bundles = new List<Bundle> { new Bundle { Id = 2, Name = "qwe" } }
        };

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
                Schedule = Schedule.ShiftWork,
                Orders = new List<Order>()
                {
                    new Order()
                    {
                        Id = 124,
                        StartTime = new DateTime(2022, 8, 1, 14, 00, 00),
                        EndTime = new DateTime(2022, 8, 1, 20, 00, 00),
                    }
                },
                DateOfStartWork = new DateTime(2022, 8, 1, 10, 00, 00)
            },
            new Cleaner()
            {
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com2",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
                Schedule = Schedule.FullTime,
                Orders = new List<Order>(),
                DateOfStartWork = new DateTime(2022, 8, 1, 10, 00, 00)
            },
            new Cleaner()
            {
                 FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com3",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
                Schedule = Schedule.ShiftWork,
                Orders = new List<Order>(),
                DateOfStartWork = new DateTime(2022, 8, 1, 10, 00, 00)
            }
        };
        _cleanersRepositoryMock.Setup(o => o.GetAllCleaners()).Returns(cleaners);
        var expectedCount = 2;

        //when
        var actual = _sut.GetFreeCleanersForOrder(order);

        //then
        Assert.NotNull(actual);
        Assert.Equal(expectedCount, actual.Count);
        _cleanersRepositoryMock.Verify(c => c.GetAllCleaners(), Times.Once);
    }
}