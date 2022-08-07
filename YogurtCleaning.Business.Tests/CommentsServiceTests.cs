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
    private Mock<IOrdersRepository> _mockOrdersRepository;

    public CommentsServiceTests()
    {
        _mockCommentsRepository = new Mock<ICommentsRepository>();
        _mockClientsRepository = new Mock<IClientsRepository>();
        _mockCleanersRepository = new Mock<ICleanersRepository>();
        _mockOrdersRepository = new Mock<IOrdersRepository>();
        _sut = new CommentsService(_mockCommentsRepository.Object, _mockClientsRepository.Object, _mockCleanersRepository.Object, _mockOrdersRepository.Object);
    }

    [Fact]
    public async Task AddCommentByClient_WhenValidRequestPassed_CommentAdded()
    {
        // given
        var expectedId = 1;

        var comment = new Comment
        {
            Summary = "qwe",
            Client = new() { Id = 1},
            Order = new() { Id = 1},
            Rating = 5
        };

        var client = new Client
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
            BirthDate = DateTime.Today
        };

        var order = new Order
        {
            Id = 1,
            Client = new() { Id = 1 },
            CleanersBand = new List<Cleaner>() { new Cleaner() { Id = 3 }, new Cleaner() { Id = 4 } },
            CleaningObject = new CleaningObject() { Id = 2 },
            StartTime = DateTime.Now,
            EndTime = DateTime.Now,
            Price = 30,
            IsDeleted = false
        };

        _mockClientsRepository.Setup(c => c.GetClient(comment.Client.Id)).ReturnsAsync(client);
        _mockOrdersRepository.Setup(o => o.GetOrder(comment.Order.Id)).ReturnsAsync(order);
        _mockCommentsRepository.Setup(c => c.AddComment(It.IsAny<Comment>())).ReturnsAsync(1);

        // when
        var actual = await _sut.AddCommentByClient(comment, comment.Client.Id);

        // then
        Assert.Equal(expectedId, actual);
        _mockCommentsRepository.Verify(c => c.AddComment(It.IsAny<Comment>()), Times.Once);
        _mockClientsRepository.Verify(c => c.GetClient(comment.Client.Id), Times.Once);
        _mockOrdersRepository.Verify(o => o.GetOrder(comment.Order.Id), Times.Once);
        _mockCleanersRepository.Verify(c => c.UpdateCleanerRating(It.IsAny<int>()), Times.Exactly(2));

    }

    [Fact]
    public async Task AddCommentByClient_WhenClientIdNotInBase_GetEntityNotFoundException()
    {
        //given
        var comment = new Comment
        {
            Summary = "qwe",
            Client = new() { Id = 1 },
            Order = new() { Id = 1 },
            Rating = 5
        };

        //when

        //then
        Assert.ThrowsAsync<Exceptions.EntityNotFoundException>(() => _sut.AddCommentByClient(comment, comment.Client.Id));
    }

    [Fact]
    public async Task AddCommentByClient_WhenOrderIdNotInBase_GetEntityNotFoundException()
    {
        //given
        var comment = new Comment
        {
            Summary = "qwe",
            Client = new() { Id = 1 },
            Order = new() { Id = 1 },
            Rating = 5
        };

        var client = new Client
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "5559997264",
            BirthDate = DateTime.Today
        };

        _mockClientsRepository.Setup(c => c.GetClient(comment.Client.Id)).ReturnsAsync(client);

        //when

        //then
        Assert.ThrowsAsync<Exceptions.EntityNotFoundException>(() => _sut.AddCommentByClient(comment, comment.Client.Id));
        _mockClientsRepository.Verify(c => c.GetClient(comment.Client.Id), Times.Once);
    }

    [Fact]
    public async Task AddCommentByCleaner_WhenValidRequestPassed_CommentAdded()
    {
        // given
        var comment = new Comment
        {
            Summary = "qwe",
            Cleaner = new() { Id = 1 },
            Order = new() { Id = 1 },
            Rating = 5
        };

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

        var order = new Order
        {
            Id = 1,
            Client = new() { Id = 1 },
            CleanersBand = new List<Cleaner>() { new Cleaner() { Id = 1 }, new Cleaner() { Id = 4 } },
            CleaningObject = new CleaningObject() { Id = 2 },
            StartTime = DateTime.Now,
            EndTime = DateTime.Now,
            Price = 30,
            IsDeleted = false
        };

        _mockCleanersRepository.Setup(c => c.GetCleaner(comment.Cleaner.Id)).ReturnsAsync(cleaner);
        _mockOrdersRepository.Setup(o => o.GetOrder(comment.Order.Id)).ReturnsAsync(order);
        _mockCommentsRepository.Setup(c => c.AddComment(It.IsAny<Comment>())).ReturnsAsync(1);

        var expectedId = 1;

        // when
        var actual = await _sut.AddCommentByCleaner(comment, comment.Cleaner.Id);

        // then
        Assert.Equal(expectedId, actual);
        _mockCommentsRepository.Verify(c => c.AddComment(It.IsAny<Comment>()), Times.Once);
        _mockCleanersRepository.Verify(c => c.GetCleaner(comment.Cleaner.Id), Times.Once);
        _mockOrdersRepository.Verify(o => o.GetOrder(comment.Order.Id), Times.Once);
        _mockClientsRepository.Verify(c => c.UpdateClientRating(comment.Order.Client.Id), Times.Once);
    }

    [Fact]
    public async Task AddCommentByCleaner_WhenCleanerIdNotInBase_GetEntityNotFoundException()
    {
        //given
        var comment = new Comment
        {
            Summary = "qwe",
            Cleaner = new() { Id = 1 },
            Order = new() { Id = 1 },
            Rating = 5
        };

        //when

        //then
        Assert.ThrowsAsync<Exceptions.EntityNotFoundException>(() => _sut.AddCommentByClient(comment, comment.Cleaner.Id));
    }

    [Fact]
    public async Task AddCommentByCleaner_WhenOrderIdNotInBase_GetEntityNotFoundException()
    {
        //given
        var comment = new Comment
        {
            Summary = "qwe",
            Cleaner = new() { Id = 1 },
            Order = new() { Id = 1 },
            Rating = 5
        };

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

        _mockCleanersRepository.Setup(c => c.GetCleaner(comment.Cleaner.Id)).ReturnsAsync(cleaner);

        //when

        //then
        Assert.ThrowsAsync<Exceptions.EntityNotFoundException>(() => _sut.AddCommentByCleaner(comment, comment.Cleaner.Id));
        _mockCleanersRepository.Verify(c => c.GetCleaner(comment.Cleaner.Id), Times.Once);
    }
}