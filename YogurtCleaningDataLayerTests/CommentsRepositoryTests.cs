using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.DataLayer.Tests;

public class CommentsRepositoryTests
{
    private readonly DbContextOptions<YogurtCleaningContext> _dbContextOptions;
    public CommentsRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<YogurtCleaningContext>()
            .UseInMemoryDatabase(databaseName: "TestDbForServices")
            .Options;
    }

    [Fact]
    public async Task AddComment_WhenCommentAdded_ThenCommentIdMoreThenZero()
    {
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CommentsRepository(context);
        var comment = new Comment
        {
            Rating = 1,
            Client = new()
            {
                Id = 1,
                Email = "c@b.c",
                FirstName = "q",
                LastName = "w",
                Password = "qweqweqweqwe",
                Phone = "89998887766"
            },
            Order = new() { Id = 1 },
            IsDeleted = false
        };

        // when
        var actual = await sut.AddComment(comment);

        //then
        Assert.True(actual > 0);
    }

    [Fact]
    public async Task DeleteComment_WhenCorrectIdPassed_ThenSoftDeleteApplied()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CommentsRepository(context);
        var comment = new Comment
        {
            Rating = 1,
            Client = new()
            {
                Id = 2,
                Email = "a@b.c",
                FirstName = "q",
                LastName = "w",
                Password = "123qweqweqweqwe",
                Phone = "99998887766"
            },
            Order = new() { Id = 2 },
            IsDeleted = false
        };

        context.Comments.Add(comment);
        context.SaveChanges();

        // when
        await sut.DeleteComment(comment);

        //then
        var actual = await sut.GetCommentById(comment.Id);
        Assert.True(actual.IsDeleted);
    }

    [Fact]
    public async Task GetAllComments_WhenCommentsExist_ThenGetComments()
    {
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CommentsRepository(context);
        var comment = new Comment
        {
            Rating = 1,
            Client = new()
            {
                Id = 3,
                Email = "b@b.c",
                FirstName = "q",
                LastName = "w",
                Password = "qweqweqweqwe43",
                Phone = "19998887766"
            },
            Order = new() { Id = 3 },
            IsDeleted = false
        };

        context.Comments.Add(comment);
        context.SaveChanges();

        // when
        var result = await sut.GetAllComments();

        //then
        Assert.Contains(comment, result);
    }

    [Fact]
    public async Task GetAllComments_WhenCommentIsDeleted_ThenCommentDoesNotGet()
    {
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CommentsRepository(context);
        var comment = new Comment
        {
            Rating = 1,
            Client = new()
            {
                Id = 4,
                Email = "b@b.c",
                FirstName = "q",
                LastName = "w",
                Password = "qweqweqweqwe43",
                Phone = "19998887766"
            },
            Order = new() { Id = 4 },
            IsDeleted = true
        };

        context.Comments.Add(comment);
        context.SaveChanges();

        // when
        var result = await sut.GetAllComments();

        //then
        Assert.DoesNotContain(comment, result);
    }
}