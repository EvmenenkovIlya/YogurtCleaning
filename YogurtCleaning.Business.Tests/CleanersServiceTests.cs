using Moq;
using YogurtCleaning.Business.Models;
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

    public CleanersServiceFacts()
    {
        _cleanersRepositoryMock = new Mock<ICleanersRepository>();
        _ordersRepositoryMock = new Mock<IOrdersRepository>();
        _sut = new CleanersService(_cleanersRepositoryMock.Object, _ordersRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateCleaner_WhenValidRequestPassed_CleanerAdded()
    {
        //given
        _cleanersRepositoryMock.Setup(c => c.CreateCleaner(It.IsAny<Cleaner>()))
             .ReturnsAsync(1);
        var expectedId = 1;

        var cleaner = new Cleaner()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com568",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
            Services = new List<Service>() { new Service() {  Id = 1} },
            Districts = new List<District>() { new District() { Id = DistrictEnum.Admiralteisky, Name = DistrictEnum.Admiralteisky.ToString() } }
        };
        _cleanersRepositoryMock.Setup(c => c.GetDistricts(cleaner.Districts))
             .ReturnsAsync(cleaner.Districts);
        _cleanersRepositoryMock.Setup(c => c.GetServices(cleaner.Services))
             .ReturnsAsync(cleaner.Services);
        //when
        var actual = await _sut.CreateCleaner(cleaner);

        //then
        Assert.Equal(expectedId, actual);
        _cleanersRepositoryMock.Verify(c => c.CreateCleaner(It.IsAny<Cleaner>()), Times.Once);
    }

    [Fact]
    public async Task CreateCleaner_WhenNotUniqueEmail_ThrowUniquenessException()
    {
        //given
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
             .ReturnsAsync(cleaners[0]);
        //when

        //then
        await Assert.ThrowsAsync<Exceptions.UniquenessException>(() => _sut.CreateCleaner(cleanerNew));
    }

    [Fact]
    public async Task GetAllCleaners_WhenValidRequestPassed_CleanersReceived()
    {
        //given
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
        _cleanersRepositoryMock.Setup(o => o.GetAllCleaners()).ReturnsAsync(cleaners);
        userValue = new UserValues() { Email = "AdamSmith@gmail.com1", Role = Role.Admin };

        //when
        var actual = await _sut.GetAllCleaners();

        //then
        Assert.NotNull(actual);
        Assert.Equal(cleaners.Count, actual.Count);
        _cleanersRepositoryMock.Verify(c => c.GetAllCleaners(), Times.Once);
    }

    [Fact]
    public async Task GetCleaner_WhenCurrentUserIsAdmin_CleanerReceived()
    {
        //given
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

        userValue = new UserValues() { Email = "AdamSmith@gmail.com1", Role = Role.Admin };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).ReturnsAsync(cleanerInDb);

        //when
        var actual = await _sut.GetCleaner(cleanerInDb.Id, userValue);

        //then
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(cleanerInDb.Id), Times.Once);
    }

    [Fact]
    public async Task GetCleaner_WhenIdNotInBase_GetEntityNotFoundException()
    {
        //given
        var testId = 2;

        var cleanerInDb = new Cleaner()
        {
            Id = 1,
            FirstName = "Adam",
        };

        userValue = new UserValues() { Email = "AdamSmith@gmail.com1", Role = Role.Admin };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).ReturnsAsync(cleanerInDb);

        //when

        //then
        await Assert.ThrowsAsync<Exceptions.EntityNotFoundException>(() => _sut.GetCleaner(testId, userValue));
    }

    [Fact]
    public async Task GetCleaner_WhenCleanerGetSomeoneElsesProfile_ThrowAccessException()
    {
        //given
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
        userValue = new UserValues() { Email = "AdamSmith@gmail.com1", Role = Role.Cleaner };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).ReturnsAsync(cleanerInDb);

        //when

        //then
        await Assert.ThrowsAsync<Exceptions.AccessException>(() => _sut.GetCleaner(cleanerInDb.Id, userValue));
    }

    [Fact]
    public async Task GetCommentsByCleaner_WhenClentGetOwnComments_CommentsReceived()
    {
        //given
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
        userValue = new UserValues() { Email = cleanerInDb.Email, Role = Role.Cleaner };

        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).ReturnsAsync(cleanerInDb);
        _cleanersRepositoryMock.Setup(o => o.GetAllCommentsByCleaner(cleanerInDb.Id)).ReturnsAsync(cleanerInDb.Comments);

        //when
        var actual = await _sut.GetCommentsByCleaner(cleanerInDb.Id, userValue);

        //then

        Assert.Equal(cleanerInDb.Comments.Count, actual.Count);
        Assert.Equal(cleanerInDb.Comments[0].Id, actual[0].Id);
        Assert.Equal(cleanerInDb.Comments[1].Id, actual[1].Id);
        Assert.Equal(cleanerInDb.Comments[0].Rating, actual[0].Rating);
        Assert.Equal(cleanerInDb.Comments[1].Rating, actual[1].Rating);
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(cleanerInDb.Id), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.GetAllCommentsByCleaner(cleanerInDb.Id), Times.Once);
    }

    [Fact]
    public async Task GetCommentsByCleaner_WhenCleanerGetSomeoneElsesComments_ThrowBadRequestException()
    {
        //given
        var cleanerInDb = new Cleaner();
        var testEmail = "FakeCleaner@gmail.ru";

        userValue = new UserValues() { Email = testEmail, Role = Role.Cleaner };

        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).ReturnsAsync(cleanerInDb);
        _cleanersRepositoryMock.Setup(o => o.GetAllCommentsByCleaner(cleanerInDb.Id)).ReturnsAsync(cleanerInDb.Comments);
        //when

        //then
        await Assert.ThrowsAsync<Exceptions.AccessException>(() => _sut.GetCommentsByCleaner(cleanerInDb.Id, userValue));
    }

    [Fact]
    public async Task GetCommentsByCleanerId_AdminGetsCommentsWhenCleanerNotInDb_ThrowBadRequestException()
    {
        //given
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
        userValue = new UserValues() { Email = testEmail, Role = Role.Admin };

        _cleanersRepositoryMock.Setup(o => o.GetAllCommentsByCleaner(cleanerInDb.Id)).ReturnsAsync(cleanerInDb.Comments);
        //when

        //then
        await Assert.ThrowsAsync<Exceptions.BadRequestException>(() => _sut.GetCommentsByCleaner(cleanerInDb.Id, userValue));
    }

    [Fact]
    public async Task GetOrdersByCleanerId_WhenClentGetsOwnOrders_OrdersReceived()
    {
        //given
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
        userValue = new UserValues() { Email = cleanerInDb.Email, Role = Role.Cleaner };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerInDb.Id)).ReturnsAsync(cleanerInDb);
        _cleanersRepositoryMock.Setup(o => o.GetAllOrdersByCleaner(cleanerInDb.Id)).ReturnsAsync(cleanerInDb.Orders);

        //when
        var actual = await _sut.GetOrdersByCleaner(cleanerInDb.Id, userValue);

        //then
        Assert.Equal(cleanerInDb.Orders.Count, actual.Count);
        Assert.Equal(cleanerInDb.Orders[0].Id, actual[0].Id);
        Assert.Equal(cleanerInDb.Orders[1].Id, actual[1].Id);
        Assert.Equal(cleanerInDb.Orders[0].Price, actual[0].Price);
        Assert.Equal(cleanerInDb.Orders[1].Price, actual[1].Price);
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(cleanerInDb.Id), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.GetAllOrdersByCleaner(cleanerInDb.Id), Times.Once);
    }

    [Fact]
    public async Task GetOrdersByCleanerId_WhenCleanersTryToGetSomeoneElsesOrders_ThrowBadRequestException()
    {
        //given
        var cleanerInDb = new Cleaner();
        var testEmail = "FakeCleaner@gmail.ru";

        userValue = new UserValues() { Email = testEmail, Role = Role.Cleaner };

        _cleanersRepositoryMock.Setup(o => o.GetAllOrdersByCleaner(cleanerInDb.Id)).ReturnsAsync(cleanerInDb.Orders);
        //when

        //then
        await Assert.ThrowsAsync<Exceptions.BadRequestException>(() => _sut.GetOrdersByCleaner(cleanerInDb.Id, userValue));
    }

    [Fact]
    public async Task GetCommentsByCleanerId_AdminGetsOrderssWhenCleanerNotInDb_ThrowBadRequestException()
    {
        //given
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
        userValue = new UserValues() { Email = testEmail, Role = Role.Admin };

        _cleanersRepositoryMock.Setup(o => o.GetAllOrdersByCleaner(cleanerInDb.Id)).ReturnsAsync(cleanerInDb.Orders);
        //when

        //then
        await Assert.ThrowsAsync<Exceptions.BadRequestException>(() => _sut.GetOrdersByCleaner(cleanerInDb.Id, userValue));
    }

    [Fact]
    public async Task UpdateCleaner_WhenCleanerUpdatesProperties_ChangesProperties()
    {
        //given
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
            Services = new() { new Service() { Id = 1}, new Service() { Id = 2 } },
            Districts = new() { new District() { Id = DistrictEnum.Kalininsky, Name = "Kalininsky" } }
        };
        userValue = new UserValues() { Email = cleaner.Email, Role = Role.Cleaner }; ;
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleaner.Id)).ReturnsAsync(cleaner);
        _cleanersRepositoryMock.Setup(o => o.GetServices(newCleanerModel.Services)).ReturnsAsync(newCleanerModel.Services);
        _cleanersRepositoryMock.Setup(o => o.GetDistricts(newCleanerModel.Districts)).ReturnsAsync(newCleanerModel.Districts);
        //when
        await _sut.UpdateCleaner(newCleanerModel, cleaner.Id, userValue);

        //then
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(cleaner.Id), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.UpdateCleaner(It.Is<Cleaner>(c =>
        c.FirstName == newCleanerModel.FirstName && c.LastName == newCleanerModel.LastName &&
        c.BirthDate == newCleanerModel.BirthDate && c.Phone == newCleanerModel.Phone &&
        c.Id == cleaner.Id && c.Email == cleaner.Email
        )), Times.Once);
    }

    [Fact]
    public async Task UpdateCleaner_WhenEmptyCleanerRequest_ThrowEntityNotFoundException()
    {
        //given
        var cleaner = new Cleaner();
        var testEmail = "FakeCleaner@gmail.ru";

        Cleaner newCleanerModel = new Cleaner()
        {
            FirstName = "Someone",
            LastName = "Else",
            Phone = "+73845277",
            BirthDate = new DateTime(1996, 10, 10)
        };
        userValue = new UserValues() { Email = testEmail, Role = Role.Cleaner };

        _cleanersRepositoryMock.Setup(o => o.UpdateCleaner(newCleanerModel));

        //when

        //then
        await Assert.ThrowsAsync<Exceptions.BadRequestException>(() => _sut.UpdateCleaner(newCleanerModel, cleaner.Id, userValue));
    }

    [Fact]
    public async Task UpdateCleaner_CleanerGetSomeoneElsesProfile_ThrowAccessException()
    {
        //given
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
        userValue = new UserValues() { Email = testEmail, Role = Role.Cleaner };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleaner.Id)).ReturnsAsync(cleaner);
        _cleanersRepositoryMock.Setup(o => o.UpdateCleaner(newCleanerModel));

        //when

        //then
        await Assert.ThrowsAsync<Exceptions.AccessException>(() => _sut.UpdateCleaner(newCleanerModel, cleaner.Id, userValue));
    }

    [Fact]
    public async Task DeleteCleaner_WhenValidRequestPassed_DeleteCleaner()
    {
        //given        
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

        _cleanersRepositoryMock.Setup(o => o.GetCleaner(expectedCleaner.Id)).ReturnsAsync(expectedCleaner);
        _cleanersRepositoryMock.Setup(o => o.DeleteCleaner(expectedCleaner));
        userValue = new UserValues() { Email = "AdamSmith@gmail.com3", Role = Role.Cleaner, Id = 1 };

        //when
        await _sut.DeleteCleaner(expectedCleaner.Id, userValue);

        //then
        _cleanersRepositoryMock.Verify(c => c.DeleteCleaner(expectedCleaner), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCleaner_EmptyCleanerRequest_ThrowEntityNotFoundException()
    {
        //given
        var testId = 1;
        var cleaner = new Cleaner();
        var testEmail = "FakeCleaner@gmail.ru";
        userValue = new UserValues() { Email = testEmail, Role = Role.Cleaner };

        //when

        //then
        await Assert.ThrowsAsync<Exceptions.BadRequestException>(() => _sut.DeleteCleaner(testId, userValue));
    }

    [Fact]
    public async Task DeleteCleaner_WhenCleanerGetSomeoneElsesProfile_ThrowAccessException()
    {
        //given
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
        userValue = new UserValues() { Email = cleanerFirst.Email, Role = Role.Cleaner };
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleanerSecond.Id)).ReturnsAsync(cleanerSecond);

        //when

        //then
        await Assert.ThrowsAsync<Exceptions.AccessException>(() => _sut.DeleteCleaner(cleanerSecond.Id, userValue));
    }

    [Fact]
    public async Task GetFreeCleanersForOrder_WhenAreFreeCleaners_ThenCleanersRecieved()
    {
        //given
        var order = new OrderBusinessModel
        {
            Client = new() { Id = 1 },
            CleaningObject = new() { Id = 56 },
            Status = Status.Created,
            StartTime = new DateTime(2022, 8, 1, 10, 00, 00),
            EndTime = new DateTime(2022, 8, 1, 18, 00, 00),
            Bundles = new List<BundleBusinessModel> { new BundleBusinessModel { Id = 2, Name = "qwe" } }
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
        _cleanersRepositoryMock.Setup(o => o.GetWorkingCleanersForDate(order.StartTime)).ReturnsAsync(cleaners);

        //when
        var actual = await _sut.GetFreeCleanersForOrder(order);

        //then
        Assert.NotNull(actual);
        Assert.Equal(cleaners.Count, actual.Count);
        _cleanersRepositoryMock.Verify(c => c.GetWorkingCleanersForDate(order.StartTime), Times.Once);
    }

    [Fact]
    public async Task GetFreeCleanersForOrder_WhenCleanerIsBusy_ThenDoNotAddToResult()
    {
        //given
        var order = new OrderBusinessModel
        {
            Client = new() { Id = 1 },
            CleaningObject = new() { Id = 56 },
            Status = Status.Created,
            StartTime = new DateTime(2022, 8, 1, 10, 00, 00),
            EndTime = new DateTime(2022, 8, 1, 18, 00, 00),
            Bundles = new List<BundleBusinessModel> { new BundleBusinessModel { Id = 2, Name = "qwe" } }
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
                Orders = new List<Order>()
                {
                    new Order()
                    {
                        Id = 124,
                        StartTime = new DateTime(2022, 8, 1, 9, 00, 00),
                        EndTime = new DateTime(2022, 8, 1, 11, 00, 00),
                    }
                },
                DateOfStartWork = new DateTime(2022, 8, 1, 10, 00, 00)
            }
        };
        _cleanersRepositoryMock.Setup(o => o.GetWorkingCleanersForDate(order.StartTime)).ReturnsAsync(cleaners);
        var expectedCount = 0;

        //when
        var actual = await _sut.GetFreeCleanersForOrder(order);

        //then
        Assert.NotNull(actual);
        Assert.Equal(expectedCount, actual.Count);
        _cleanersRepositoryMock.Verify(c => c.GetWorkingCleanersForDate(order.StartTime), Times.Once);
    }

    [Fact]
    public async Task UpdateCleanerRatingTest_WhenValidRequestGet_ThenRatingUpdated()
    {
        // given
        var cleaner = new Cleaner
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Email = "ccc@gmail.c",
            Password = "1234qwerty",
            Passport = "0000654321",
            Phone = "89998887766",
            IsDeleted = false
        };
        var comments = new List<Comment>()
        {
            new Comment { Id = 1, Order = new(){Id = 1}, Client = new(){Id = 1}, Rating = 1 },
            new Comment { Id = 3, Order = new(){Id = 2}, Client = new(){Id = 2}, Rating = 2 }
        };

        _cleanersRepositoryMock.Setup(c => c.GetCleaner(cleaner.Id)).ReturnsAsync(cleaner);
        _cleanersRepositoryMock.Setup(c => c.GetCommentsAboutCleaner(cleaner.Id)).ReturnsAsync(comments);

        var expectedRating = 1.5m;

        // when
        await _sut.UpdateCleanerRating(cleaner.Id);

        // then

        _cleanersRepositoryMock.Verify(c => c.UpdateCleaner(
            It.Is<Cleaner>(
                i => i.Id == cleaner.Id
                && i.Rating == expectedRating)),
            Times.Once);
        _cleanersRepositoryMock.Verify(c => c.GetCleaner(cleaner.Id), Times.Once);
        _cleanersRepositoryMock.Verify(c => c.GetCommentsAboutCleaner(cleaner.Id), Times.Once);
    }
}