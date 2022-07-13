using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaningDataLayerTests;

public class CommentsRepositoryTests
{
    private readonly DbContextOptions<YogurtCleaningContext> _dbContextOptions;
    public CommentsRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<YogurtCleaningContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
    }

    [Fact]
    public void AddComment_WhenCommentAdded_ThenCommentIdMoreThenZero()
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
        context.Comments.Add(comment);
        context.SaveChanges();

        //then
        Assert.True(comment.Id > 0);
    }

    [Fact]
    public void DeleteComment_WhenCorrectIdPassed_ThenSoftDeleteApplied()
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
        sut.DeleteComment(comment.Id);

        //then
        Assert.True(comment.IsDeleted);
    }

    [Fact]
    public void GetAllComments_WhenCommentsExist_ThenGetComments()
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
        var result = sut.GetAllComments();

        //then
        Assert.True(result.Contains(comment));
    }

    [Fact]
    public void GetAllComments_WhenCommentIsDeleted_ThenCommentDoesNotGet()
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
        var result = sut.GetAllComments();

        //then
        Assert.False(result.Contains(comment));
    }
}