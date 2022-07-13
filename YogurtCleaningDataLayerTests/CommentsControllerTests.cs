using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaningDataLayerTests;

public class CommentsControllerTests
{
    private readonly DbContextOptions<YogurtCleaningContext> _dbContextOptions;
    public CommentsControllerTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<YogurtCleaningContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
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
            Client = new() { Id = 1, Email = "a@b.c", FirstName = "q", LastName = "w", Password = "qweqweqweqwe", Phone = "89998887766" },
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
}