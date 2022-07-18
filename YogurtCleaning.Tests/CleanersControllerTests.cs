using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.Controllers;
using YogurtCleaning.Models;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Tests;
public class CleanersControllerTests
{
    private CleanersController _sut;
    private Mock<ICleanersService> CleanersServiceMock;
    private Mock<IMapper> _mapper;

    private List<string> _identities;

    [SetUp]
    public void Setup()
    {
        _mapper = new Mock<IMapper>();
        CleanersServiceMock = new Mock<ICleanersService>();
        _sut = new CleanersController(_mapper.Object, CleanersServiceMock.Object);
    }

    [Test]
    public async Task CreateCleaner_WhenValidRequestPassed_CreatedResultReceived()
    {
        //given
        CleanersServiceMock.Setup(c => c.CreateCleaner(It.IsAny<Cleaner>()))
         .Returns(1);

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
        var actual = _sut.AddCleaner(cleaner);
        var a = actual.Result;

        //then
        var actualResult = actual.Result as CreatedResult;

        Assert.AreEqual(StatusCodes.Status201Created, actualResult.StatusCode);
        Assert.True((int)actualResult.Value == 1);

        CleanersServiceMock.Verify(c => c.CreateCleaner(It.IsAny<Cleaner>()), Times.Once);
        CleanersServiceMock.Verify(c => c.DeleteCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetAllCleaners(It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.UpdateCleaner(It.IsAny<Cleaner>(), It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetOrdersByCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetCommentsByCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
    }

    [Test]
    public void GetCleaner_WhenValidRequestPassed_OkReceived()
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
        CleanersServiceMock.Setup(o => o.GetCleaner(expectedCleaner.Id, _identities)).Returns(expectedCleaner);

        //when
        var actual = _sut.GetCleaner(expectedCleaner.Id);

        //then
        var actualResult = actual.Result as ObjectResult;


        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        CleanersServiceMock.Verify(c => c.CreateCleaner(It.IsAny<Cleaner>()), Times.Never);
        CleanersServiceMock.Verify(c => c.DeleteCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Once);
        CleanersServiceMock.Verify(c => c.GetAllCleaners(It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.UpdateCleaner(It.IsAny<Cleaner>(), It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetOrdersByCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetCommentsByCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
    }

    [Test]
    public void UpdateCleaner_WhenValidRequestPassed_NoContentReceived()
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

        CleanersServiceMock.Setup(o => o.UpdateCleaner(cleaner, cleaner.Id, _identities));

        //when
        var actual = _sut.UpdateCleaner(cleaner.Id, newCleanerModel);

        //then
        var actualResult = actual as NoContentResult;

        Assert.AreEqual(StatusCodes.Status204NoContent, actualResult.StatusCode);
        CleanersServiceMock.Verify(c => c.CreateCleaner(It.IsAny<Cleaner>()), Times.Never);
        CleanersServiceMock.Verify(c => c.DeleteCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetAllCleaners(It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.UpdateCleaner(It.IsAny<Cleaner>(), It.IsAny<int>(), It.IsAny<List<string>>()), Times.Once);
        CleanersServiceMock.Verify(c => c.GetOrdersByCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetCommentsByCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
    }

    [Test]
    public void GetCommentsByCleanerId_WhenValidRequestPassed_RequestedTypeReceived()
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
        CleanersServiceMock.Setup(o => o.GetCommentsByCleaner(expectedCleaner.Id, _identities)).Returns(expectedCleaner.Comments);

        //when
        var actual = _sut.GetAllCommentsByCleaner(expectedCleaner.Id);

        //then
        var actualResult = actual.Result as ObjectResult;

        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        CleanersServiceMock.Verify(c => c.CreateCleaner(It.IsAny<Cleaner>()), Times.Never);
        CleanersServiceMock.Verify(c => c.DeleteCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetAllCleaners(It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.UpdateCleaner(It.IsAny<Cleaner>(), It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetOrdersByCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetCommentsByCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Once);
    }

    [Test]
    public void GetOrdersByCleaner_WhenValidRequestPassed_RequestedTypeReceived()
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
                    Id = 1, Price = 124, Status = DataLayer.Enums.Status.Created
                },
                new()
                {
                    Id = 2, Price = 1245, Status = DataLayer.Enums.Status.Done
                }
            },

        };

        CleanersServiceMock.Setup(o => o.GetOrdersByCleaner(expectedCleaner.Id, _identities)).Returns(expectedCleaner.Orders);

        //when
        var actual = _sut.GetAllOrdersByCleaner(expectedCleaner.Id);

        //then
        var actualResult = actual.Result as ObjectResult;

        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        CleanersServiceMock.Verify(c => c.CreateCleaner(It.IsAny<Cleaner>()), Times.Never);
        CleanersServiceMock.Verify(c => c.DeleteCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetAllCleaners(It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.UpdateCleaner(It.IsAny<Cleaner>(), It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetOrdersByCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Once);
        CleanersServiceMock.Verify(c => c.GetCommentsByCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
    }

    [Test]
    public void DeleteCleanerById_WhenValidRequestPassed_NoContentReceived()
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

        CleanersServiceMock.Setup(o => o.GetCleaner(expectedCleaner.Id, _identities)).Returns(expectedCleaner);

        //when
        var actual = _sut.DeleteCleaner(expectedCleaner.Id);

        //then
        var actualResult = actual as NoContentResult;

        Assert.AreEqual(StatusCodes.Status204NoContent, actualResult.StatusCode);
        CleanersServiceMock.Verify(c => c.CreateCleaner(It.IsAny<Cleaner>()), Times.Never);
        CleanersServiceMock.Verify(c => c.DeleteCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Once);
        CleanersServiceMock.Verify(c => c.GetCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetAllCleaners(It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.UpdateCleaner(It.IsAny<Cleaner>(), It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetOrdersByCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetCommentsByCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
    }

    [Test]
    public void GetAllCleaners_WhenValidRequestPassed_RequestedTypeReceived()
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
                IsDeleted = true,
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

        CleanersServiceMock.Setup(o => o.GetAllCleaners(_identities)).Returns(cleaners).Verifiable();

        //when
        var actual = _sut.GetAllCleaners();

        //then
        var actualResult = actual.Result as ObjectResult;

        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        CleanersServiceMock.Verify(c => c.CreateCleaner(It.IsAny<Cleaner>()), Times.Never);
        CleanersServiceMock.Verify(c => c.DeleteCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetAllCleaners(It.IsAny<List<string>>()), Times.Once);
        CleanersServiceMock.Verify(c => c.UpdateCleaner(It.IsAny<Cleaner>(), It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetOrdersByCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        CleanersServiceMock.Verify(c => c.GetCommentsByCleaner(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
    }
}