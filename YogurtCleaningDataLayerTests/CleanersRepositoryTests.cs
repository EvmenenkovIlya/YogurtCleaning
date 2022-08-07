using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.DataLayer.Tests;

public class CleanersRepositoryTests
{
    private readonly DbContextOptions<YogurtCleaningContext> _dbContextOptions;
    public CleanersRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<YogurtCleaningContext>()
            .UseInMemoryDatabase(databaseName: "TestDbForCleaners")
            .Options;
    }

    [Fact]
    public async Task AddCleaner_WhenCleanerAdded_ThenCommentIdMoreThenZero()
    {
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CleanersRepository(context);
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

        // when
        var actual = await sut.CreateCleaner(cleaner);

        //then
        Assert.True(actual > 0);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task DeleteCleaner_WhenCorrectIdPassed_ThenSoftDeleteApplied()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CleanersRepository(context);
        var cleaner = new Cleaner
        {
            Id = 2,
            FirstName = "Adam",
            LastName = "Smith",
            Email = "ccc@gmail.c",
            Password = "1234qwerty",
            Passport = "0000654321",
            Phone = "89998887766",
            IsDeleted = false
        };

        context.Cleaners.Add(cleaner);
        context.SaveChanges();

        // when
        await sut.DeleteCleaner(cleaner);

        //then
        var actual = await sut.GetCleaner(cleaner.Id);
        Assert.True(actual.IsDeleted);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllCleaners_WhenCleanersExist_ThenGetCleaners()
    {
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CleanersRepository(context);
        var cleaners = new List<Cleaner>()
        {
            new Cleaner()
            {
                Id = 3,
                FirstName = "Adam",
                LastName = "Smith",
                Email = "ccc@gmail.c",
                Password = "1234qwerty",
                Passport = "0000654321",
                Phone = "89998887766",
                IsDeleted = false
            },
            new Cleaner()
            {
                Id = 4,
                FirstName = "Madara",
                LastName = "Smith",
                Email = "ychiha@gmail.japan",
                Password = "1234qwerty",
                Passport = "0000654321",
                Phone = "899988873456",
                IsDeleted = false
            }
        };

        context.Cleaners.AddRange(cleaners);
        context.SaveChanges();

        // when
        var result = await sut.GetAllCleaners();

        //then
        Assert.NotNull(result);
        Assert.Equal(typeof(List<Cleaner>), result.GetType());
        Assert.Null(result[0].Comments);
        Assert.Null(result[1].Orders);
        Assert.False(result[0].IsDeleted);
        Assert.False(result[1].IsDeleted);
        Assert.NotNull(result.Find(x => x.FirstName == "Madara"));
        Assert.NotNull(result.Find(x => x.FirstName == "Adam"));
        Assert.Null(result.Find(x => x.FirstName == "Ilya"));
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllCleaners_WhenCleanerIsDeleted_ThenCleanerDoesNotGet()
    {
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CleanersRepository(context);
        var cleaners = new List<Cleaner>()
        {
            new Cleaner()
            {
                Id = 5,
                FirstName = "Adam",
                LastName = "Smith",
                Email = "ccc@gmail.c",
                Password = "1234qwerty",
                Passport = "0000654321",
                Phone = "89998887766",
                IsDeleted = false
            },
            new Cleaner()
            {
                Id = 6,
                FirstName = "Madara",
                LastName = "Smith",
                Email = "ychiha@gmail.japan",
                Password = "1234qwerty",
                Passport = "0000654321",
                Phone = "899988873456",
                IsDeleted = true
            }
        };

        context.Cleaners.AddRange(cleaners);
        context.SaveChanges();

        // when
        var result = await sut.GetAllCleaners();

        //then
        Assert.NotNull(result);
        Assert.Equal(typeof(List<Cleaner>), result.GetType());
        Assert.Equal(1, result.Count);
        Assert.Null(result[0].Comments);
        Assert.Null(result[0].Orders);
        Assert.False(result[0].IsDeleted);
        Assert.Null(result.Find(x => x.FirstName == "Madara"));
        Assert.NotNull(result.Find(x => x.FirstName == "Adam"));
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllCommentsByCleaner_WhenCommetsGet_ThenCommentsGet()
    {
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CleanersRepository(context);
        var cleaner = new Cleaner
        {
            Id = 7,
            FirstName = "Adam",
            LastName = "Smith",
            Email = "ccc@gmail.c",
            Password = "1234qwerty",
            Passport = "0000654321",
            Phone = "89998887766",
            IsDeleted = false,
            Comments = new List<Comment>()
            {
                new Comment()
                {
                    Rating = 1,
                    Order = new() { Id = 1 },
                    IsDeleted = true
                },
                new Comment()
                {
                    Rating = 5,
                    Order = new() { Id = 2 },
                    IsDeleted = false
                }
            }
        };
        context.Cleaners.Add(cleaner);
        context.SaveChanges();

        // when
        var result = await sut.GetAllCommentsByCleaner(cleaner.Id);

        //then
        Assert.NotNull(result);
        Assert.Equal(typeof(List<Comment>), result.GetType());
        Assert.True(result[0].IsDeleted);
        Assert.False(result[1].IsDeleted);
        Assert.NotNull(result.Find(x => x.Rating == 5));
        Assert.NotNull(result.Find(x => x.Order.Id == 1));
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task UpdateCleaner_WhenCleanerUpdated_ThenCleanerDoesNotHaveOldProperty()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CleanersRepository(context);
        var cleaner = new Cleaner
        {

            Id = 8,
            FirstName = "Adam",
            LastName = "Smith",
            Email = "ccc@gmail.c",
            Password = "1234qwerty",
            Passport = "0000654321",
            Phone = "89998887766",
            IsDeleted = false

        };
        context.Cleaners.Add(cleaner);
        context.SaveChanges();
        cleaner.FirstName = "Vasya";
        cleaner.LastName = "Pupkin";

        //when
        await sut.UpdateCleaner(cleaner);
        var result = await sut.GetCleaner(cleaner.Id);

        //then
        Assert.NotEqual("Adam", result.FirstName);
        Assert.NotEqual("Smith", result.LastName);
        Assert.Equal("ccc@gmail.c", result.Email);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task UpdateCleanerRatingTest_WhenValidRequestPassed_ThenRatingUpdated()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CleanersRepository(context);

        var cleaner = new Cleaner
        {
            Id = 8,
            FirstName = "Adam",
            LastName = "Smith",
            Email = "ccc@gmail.c",
            Password = "1234qwerty",
            Passport = "0000654321",
            Phone = "89998887766",
            Rating = 0,
            IsDeleted = false
        };

        var cleanersBand = new List<Cleaner>();
        cleanersBand.Add(cleaner);

        var client = new Client { Id = 7, Email = "a@b.c", FirstName = "asdfg", LastName = "dfdfdfd", Password = "kjha", Phone = "1234567890" };
        var order1 = new Order { Id = 1, CleanersBand = cleanersBand };
        var order2 = new Order { Id = 2, CleanersBand = cleanersBand };
        var comment1 = new Comment { Id = 1, Order = order1, Client = client, Rating = 5};
        var comment2 = new Comment { Id = 2, Order = order2, Client = client, Rating = 4 };
        var comment3 = new Comment { Id = 3, Order = order2, Cleaner = cleaner, Rating = 5 };

        context.Cleaners.Add(cleaner);
        context.SaveChanges();
        context.Clients.Add(client);
        context.SaveChanges();
        context.Orders.Add(order1);
        context.SaveChanges();
        context.Orders.Add(order2);
        context.SaveChanges();
        context.Comments.Add(comment1);
        context.SaveChanges();
        context.Comments.Add(comment2);
        context.SaveChanges();
        context.Comments.Add(comment3);
        context.SaveChanges();

        var expectedRating = 4.5m;

        // when
        await sut.UpdateCleanerRating(cleaner.Id);
        var result = await sut.GetCleaner(cleaner.Id);

        // then
        Assert.Equal(expectedRating, result.Rating);
        context.Database.EnsureDeleted();
    }
}
