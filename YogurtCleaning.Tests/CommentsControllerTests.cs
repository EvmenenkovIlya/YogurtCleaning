using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.Controllers;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.Models;

namespace YogurtCleaning.API.Tests;

public class CommentsControllerTests
{
    private CommentsController _sut;
    private Mock<ICommentsService> _mockCommentsService;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _mockCommentsService = new Mock<ICommentsService>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigStorage>()));
        _sut = new CommentsController(_mockCommentsService.Object, _mapper);
    }

    [Test]
    public async Task AddCommentByClient_WhenValidRequestPassed_ThenCreatedResultRecived()
    {
        // given
        int expectedId = 1;
        _mockCommentsService.Setup(o => o.AddCommentByClient(It.IsAny<Comment>(), It.IsAny<int>())).ReturnsAsync(expectedId);
        var comment = new CommentRequest()
        {
            Summary = "ok",
            OrderId = 1,
            Rating = 5
        };

        // when
        var actual = await _sut.AddCommentByClient(comment);

        // then
        var actualResult = actual.Result as CreatedResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That((int)actualResult.Value, Is.EqualTo(expectedId));
        _mockCommentsService.Verify(o => o.AddCommentByClient(It.IsAny<Comment>(), It.IsAny<int>()), Times.Once);
    }

    [Test]
    public async Task AddCommentByCleaner_WhenValidRequestPassed_ThenCreatedResultRecived()
    {
        // given
        int expectedId = 1;
        _mockCommentsService.Setup(o => o.AddCommentByCleaner(It.IsAny<Comment>(), It.IsAny<int>())).ReturnsAsync(expectedId);
        var comment = new CommentRequest()
        {
            Summary = "ok",
            OrderId = 1,
            Rating = 5
        };

        // when
        var actual = await _sut.AddCommentByCleaner(comment);

        // then
        var actualResult = actual.Result as CreatedResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That((int)actualResult.Value, Is.EqualTo(expectedId));
        _mockCommentsService.Verify(o => o.AddCommentByCleaner(It.IsAny<Comment>(), It.IsAny<int>()), Times.Once);
    }

    [Test]
    public async Task GetAllCommentsTest_WhenValidRequestPassed_ThenOkResultRecieved()
    {
        // given
        var comments = new List<Comment>
        {
            new() 
            {
                Rating = 1,
                Client = new() {Id = 1},
                Order = new() {Id = 3},
                IsDeleted = false
            },
            new()
            {
                Rating = 5,
                Summary = "asjhdagldhsjg",
                Cleaner = new() {Id = 3},
                Order = new() {Id = 43},
                IsDeleted = false
            }

        };
        _mockCommentsService.Setup(c => c.GetComments()).ReturnsAsync(comments);

        // when
        var actual = await _sut.GetAllComments();

        // then
        var actualResult = actual.Result as ObjectResult;
        var commentsResponse = actualResult.Value as List<CommentResponse>;
        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.Multiple(() =>
        {
            Assert.That(commentsResponse.Count, Is.EqualTo(comments.Count));
            Assert.That(commentsResponse[0].Summary, Is.EqualTo(comments[0].Summary));
            Assert.That(commentsResponse[0].ClientId, Is.EqualTo(comments[0].Client.Id));
            Assert.That(commentsResponse[1].Rating, Is.EqualTo(comments[1].Rating));
        });
        _mockCommentsService.Verify(c => c.GetComments(), Times.Once);
    }
}