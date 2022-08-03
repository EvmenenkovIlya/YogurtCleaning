using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;
using Moq;

namespace YogurtCleaning.Business.Tests;

public class CommentsServiceTests
{
    private CommentsService _sut;
    private Mock<ICommentsRepository> _mockCommentsRepository;
    private Mock<IClientsRepository> _mockClientsRepository;
    private Mock<ICleanersRepository> _mockCleanersRepository;

    public CommentsServiceTests()
    {
        _mockCommentsRepository = new Mock<ICommentsRepository>();
        _mockClientsRepository = new Mock<IClientsRepository>();
        _mockCleanersRepository = new Mock<ICleanersRepository>();
        _sut = new CommentsService(_mockCommentsRepository.Object, _mockClientsRepository.Object, _mockCleanersRepository.Object);
    }

    [Fact]
    public async Task AddCommentByClient_WhenValidRequestPassed_CommentAdded()
    {
        // given
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
        var actual = await _sut.AddCommentByClient(comment, comment.Client.Id);

        // then
        Assert.Equal(expectedId, actual);
        _mockCommentsRepository.Verify(c => c.AddComment(It.IsAny<Comment>()), Times.Once);

    }

    [Fact]
    public async Task AddCommentByCleaner_WhenValidRequestPassed_CommentAdded()
    {
        // given
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
        var actual = await _sut.AddCommentByCleaner(comment, comment.Cleaner.Id);

        // then
        Assert.Equal(expectedId, actual);
        _mockCommentsRepository.Verify(c => c.AddComment(It.IsAny<Comment>()), Times.Once);       
    }
}