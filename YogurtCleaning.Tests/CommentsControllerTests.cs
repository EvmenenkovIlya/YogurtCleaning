using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.Controllers;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.Models;

namespace YogurtCleaning.API.Tests;

public class CommentsControllerTests
{
    private CommentsController _sut;
    private Mock<ILogger<CommentsController>> _mockLogger;
    private Mock<ICommentsService> _mockCommentsService;
    private Mock<IMapper> _mockMapper;

    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<CommentsController>>();
        _mockCommentsService = new Mock<ICommentsService>();
        _mockMapper = new Mock<IMapper>();
        _sut = new CommentsController(_mockLogger.Object, _mockCommentsService.Object, _mockMapper.Object);
    }

    [Test]
    public void AddCommentByClient_WhenValidRequestPassed_ThenCreatedResultRecived()
    {
        // given
        _mockCommentsService.Setup(o => o.AddCommentByClient(It.IsAny<Comment>(), It.IsAny<int>())).Returns(1);
        var comment = new CommentRequest()
        {
            Summary = "ok",
            OrderId = 1,
            Rating = 5
        };

        // when
        var actual = _sut.AddCommentByClient(comment);

        // then
        var actualResult = actual.Result as CreatedResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.True((int)actualResult.Value == 1);
        _mockCommentsService.Verify(o => o.AddCommentByClient(It.IsAny<Comment>(), It.IsAny<int>()), Times.Once);
    }

    [Test]
    public void AddCommentByCleaner_WhenValidRequestPassed_ThenCreatedResultRecived()
    {
        // given
        _mockCommentsService.Setup(o => o.AddCommentByCleaner(It.IsAny<Comment>(), It.IsAny<int>())).Returns(1);
        var comment = new CommentRequest()
        {
            Summary = "ok",
            OrderId = 1,
            Rating = 5
        };

        // when
        var actual = _sut.AddCommentByCleaner(comment);

        // then
        var actualResult = actual.Result as CreatedResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.True((int)actualResult.Value == 1);
        _mockCommentsService.Verify(o => o.AddCommentByCleaner(It.IsAny<Comment>(), It.IsAny<int>()), Times.Once);
    }
}

    