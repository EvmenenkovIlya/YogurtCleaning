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
public class CleanersControllerTests
{
    private CleanersController _sut;
    private Mock<ICleanersService> _cleanersServiceMock;
    private IMapper _mapper;
    private UserValues _userValues;

    [SetUp]
    public void Setup()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigStorage>()));
        _cleanersServiceMock = new Mock<ICleanersService>();
        _sut = new CleanersController(_mapper, _cleanersServiceMock.Object);
        _userValues = new UserValues();
    }

    [Test]
    public async Task CreateCleaner_WhenValidRequestPassed_CreatedResultReceived()
    {
        //given
        _cleanersServiceMock.Setup(c => c.CreateCleaner(It.IsAny<Cleaner>()))
         .ReturnsAsync(1);

        var cleaner = new CleanerRegisterRequest()
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
        var actual = await _sut.AddCleaner(cleaner);

        //then
        var actualResult = actual.Result as CreatedResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That((int)actualResult.Value, Is.EqualTo(1));
        _cleanersServiceMock.Verify(x => x.CreateCleaner(It.Is<Cleaner>(c => c.FirstName == cleaner.FirstName &&
        c.LastName == cleaner.LastName && c.Password == cleaner.Password && c.Email == cleaner.Email &&
        c.Phone == cleaner.Phone && c.BirthDate == cleaner.BirthDate
        )), Times.Once);
    }

    [Test]
    public async Task GetCleaner_WhenValidRequestPassed_OkReceived()
    {
        //given
        var expectedCleaner = new Cleaner()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            BirthDate = DateTime.Today
        };

        _cleanersServiceMock.Setup(o => o.GetCleaner(expectedCleaner.Id, It.IsAny<UserValues>())).ReturnsAsync(expectedCleaner);

        //when
        var actual = await _sut.GetCleaner(expectedCleaner.Id);

        //then

        var actualResult = actual.Result as ObjectResult;
        var cleanerResponse = actualResult.Value as CleanerResponse;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.Multiple(() =>
        {
            Assert.That(cleanerResponse.FirstName, Is.EqualTo(expectedCleaner.FirstName)); 
            Assert.That(cleanerResponse.LastName, Is.EqualTo(expectedCleaner.LastName));
            Assert.That(cleanerResponse.Email, Is.EqualTo(expectedCleaner.Email));
            Assert.That(cleanerResponse.Phone, Is.EqualTo(expectedCleaner.Phone));
            Assert.That(cleanerResponse.BirthDate, Is.EqualTo(expectedCleaner.BirthDate));
        });
        _cleanersServiceMock.Verify(x => x.GetCleaner(expectedCleaner.Id, It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public async Task UpdateCleaner_WhenValidRequestPassed_NoContentReceived()
    {
        //given

        var cleaner = new Cleaner()
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com",
            Phone = "85559997264",
            BirthDate = DateTime.Today
        };

        var newCleanerModel = new CleanerUpdateRequest()
        {
            FirstName = "Valid",
            LastName = "Person",
            Phone = "893048827534",
            BirthDate = DateTime.Today
        };

        _cleanersServiceMock.Setup(o => o.UpdateCleaner(cleaner, cleaner.Id, _userValues));

        //when
        var actual = await _sut.UpdateCleaner(newCleanerModel, cleaner.Id);

        //then
        var actualResult = actual as NoContentResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        _cleanersServiceMock.Verify(c => c.UpdateCleaner(It.Is<Cleaner>(c => c.FirstName == newCleanerModel.FirstName &&
        c.LastName == newCleanerModel.LastName && c.Email == null &&
        c.BirthDate == newCleanerModel.BirthDate && c.Phone == newCleanerModel.Phone), It.Is<int>(i => i == cleaner.Id), It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public async Task GetCommentsByCleanerId_WhenValidRequestPassed_RequestedTypeReceived()
    {
        //given
        var expectedCleaner = new Cleaner()
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
        _cleanersServiceMock.Setup(o => o.GetCommentsByCleaner(expectedCleaner.Id, It.IsAny<UserValues>())).ReturnsAsync(expectedCleaner.Comments);

        //when
        var actual = await _sut.GetAllCommentsByCleaner(expectedCleaner.Id);

        //then
        var actualResult = actual.Result as ObjectResult;
        var commentsResponse = actualResult.Value as List<CommentResponse>;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.Multiple(() =>
        {
            Assert.That(commentsResponse.Count, Is.EqualTo(expectedCleaner.Comments.Count));
            Assert.That(commentsResponse[0].Id, Is.EqualTo(expectedCleaner.Comments[0].Id));
            Assert.That(commentsResponse[1].Summary, Is.EqualTo(expectedCleaner.Comments[1].Summary));
            Assert.That(commentsResponse[0].Rating, Is.EqualTo(expectedCleaner.Comments[0].Rating));
        });
        _cleanersServiceMock.Verify(c => c.GetCommentsByCleaner(expectedCleaner.Id, It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public async Task GetOrdersByCleaner_WhenValidRequestPassed_RequestedTypeReceived()
    {
        //given
        var expectedCleaner = new Cleaner()
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

        _cleanersServiceMock.Setup(o => o.GetOrdersByCleaner(expectedCleaner.Id, It.IsAny<UserValues>())).ReturnsAsync(expectedCleaner.Orders);

        //when
        var actual = await _sut.GetAllOrdersByCleaner(expectedCleaner.Id);

        //then
        var actualResult = actual.Result as ObjectResult;
        var ordersResponse = actualResult.Value as List<OrderResponse>;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.Multiple(() =>
        {
            Assert.That(ordersResponse.Count, Is.EqualTo(expectedCleaner.Orders.Count));
            Assert.That(ordersResponse[0].Id, Is.EqualTo(expectedCleaner.Orders[0].Id));
            Assert.That(ordersResponse[1].Price, Is.EqualTo(expectedCleaner.Orders[1].Price));
            Assert.That(ordersResponse[0].Status, Is.EqualTo(expectedCleaner.Orders[0].Status));
        });
        _cleanersServiceMock.Verify(c => c.GetOrdersByCleaner(It.IsAny<int>(), It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public async Task DeleteCleanerById_WhenValidRequestPassed_NoContentReceived()
    {
        //given
        var expectedCleaner = new Cleaner()
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

        _cleanersServiceMock.Setup(o => o.GetCleaner(expectedCleaner.Id, _userValues)).ReturnsAsync(expectedCleaner);

        //when
        var actual = await _sut.DeleteCleaner(expectedCleaner.Id);

        //then
        var actualResult = actual as NoContentResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        _cleanersServiceMock.Verify(c => c.DeleteCleaner(It.IsAny<int>(), It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public async Task GetAllCleaners_WhenValidRequestPassed_RequestedTypeReceived()
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
                BirthDate = DateTime.Today
            },
            new Cleaner()
            {
                FirstName = "Adam1",
                LastName = "Smith1",
                Password = "123456781",
                Email = "AdamSmith@gmail.com1",
                Phone = "855599972641",
                BirthDate = DateTime.Today,
            },
            new Cleaner()
            {
                FirstName = "Adam2",
                LastName = "Smith2",
                Password = "123456782",
                Email = "AdamSmith@gmail.com2",
                Phone = "855599972642",
                BirthDate = DateTime.Today,
            }
        };

        _cleanersServiceMock.Setup(o => o.GetAllCleaners()).ReturnsAsync(cleaners).Verifiable();

        //when
        var actual = await _sut.GetAllCleaners();

        //then
        var actualResult = actual.Result as ObjectResult;
        var cleanersResponse = actualResult.Value as List<CleanerResponse>;


        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.Multiple(() =>
        {
            Assert.That(cleanersResponse.Count, Is.EqualTo(cleaners.Count));
            Assert.That(cleanersResponse[0].FirstName, Is.EqualTo(cleaners[0].FirstName));
            Assert.That(cleanersResponse[1].LastName, Is.EqualTo(cleaners[1].LastName));
            Assert.That(cleanersResponse[2].Email, Is.EqualTo(cleaners[2].Email));
            Assert.That(cleanersResponse[1].Phone, Is.EqualTo(cleaners[1].Phone));
            Assert.That(cleanersResponse[0].BirthDate, Is.EqualTo(cleaners[0].BirthDate));
        });
        _cleanersServiceMock.Verify(x => x.GetAllCleaners(), Times.Once);
    }
}