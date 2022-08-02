﻿using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Facts;

public class CleanersServiceFacts
{
    private CleanersService _sut;
    private Mock<ICleanersRepository> _cleanersRepositoryMock;
    private UserValues userValue;

    public CleanersServiceFacts()
    {
        _cleanersRepositoryMock = new Mock<ICleanersRepository>();
        _sut = new CleanersService(_cleanersRepositoryMock.Object);
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
        };

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
        var actual = _sut.GetCleaner(cleanerInDb.Id, userValue);

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
        };
        userValue = new UserValues() { Email = cleaner.Email, Role = Role.Cleaner }; ;
        _cleanersRepositoryMock.Setup(o => o.GetCleaner(cleaner.Id)).ReturnsAsync(cleaner);
        _cleanersRepositoryMock.Setup(o => o.UpdateCleaner(newCleanerModel));

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
}