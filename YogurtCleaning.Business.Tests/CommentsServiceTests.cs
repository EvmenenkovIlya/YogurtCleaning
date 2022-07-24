using Microsoft.EntityFrameworkCore;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;
using Moq;

namespace YogurtCleaning.Business.Tests;

public class CommentsServiceTests
{
    private CommentsService _sut;
    private Mock<ICommentsRepository> _mockCommentsRepository;
    private Mock<IClientsRepository> _mockClientsRepository;
    private Mock<ICleanersRepository> _mockCleanersRepository;

    private void Setup()
    {
        _mockCommentsRepository = new Mock<ICommentsRepository>();
        _mockClientsRepository = new Mock<IClientsRepository>();
        _mockCleanersRepository = new Mock<ICleanersRepository>();
        _sut = new CommentsService(_mockCommentsRepository.Object, _mockClientsRepository.Object, _mockCleanersRepository.Object);
    }


    [Fact]
    public void AddCommentByClient_WhenValidRequestPassed_CommentAdded()
    {
        // given
        Setup();
        _mockCommentsRepository.Setup(c => c.AddComment(It.IsAny<Comment>())).Returns(1);
        
        var expectedId = 1;

        var comment = new Comment
        {
            Summary = "qwe",
            Client = new() { Id = 1},
            Order = new() { Id = 1},
            Rating = 5
        };



        // when
        var actual = _sut.AddCommentByClient(comment, comment.Client.Id);

        // then
        Assert.True(actual == expectedId);
        _mockCommentsRepository.Verify(c => c.AddComment(It.IsAny<Comment>()), Times.Once);

    }

    [Fact]
    public void AddCommentByCleaner_WhenValidRequestPassed_CommentAdded()
    {
        // given
        Setup();
        _mockCommentsRepository.Setup(c => c.AddComment(It.IsAny<Comment>())).Returns(1);

        var expectedId = 1;

        var comment = new Comment
        {
            Summary = "qwe",
            Cleaner = new() { Id = 1 },
            Order = new() { Id = 1 },
            Rating = 5
        };



        // when
        var actual = _sut.AddCommentByCleaner(comment, comment.Cleaner.Id);

        // then
        Assert.True(actual == expectedId);
        _mockCommentsRepository.Verify(c => c.AddComment(It.IsAny<Comment>()), Times.Once);

    }
}