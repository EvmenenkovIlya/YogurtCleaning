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
    private Mock<ICommentsService> _mockCommentsService;
    private Mock<IMapper> _mockMapper;

    [SetUp]
    public void Setup()
    {
        _mockCommentsService = new Mock<ICommentsService>();
        _mockMapper = new Mock<IMapper>();
        _sut = new CommentsController(_mockCommentsService.Object, _mockMapper.Object);
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
}