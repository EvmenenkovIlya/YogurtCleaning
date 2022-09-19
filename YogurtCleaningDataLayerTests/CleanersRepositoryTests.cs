using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
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
        Assert.True(actual!.IsDeleted);
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
                Orders = new(){new(){Id = 1}},
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
                Orders = new(){new(){Id = 2}},
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
                Orders = new(){new(){Id = 1}},
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
                Orders = new(){new(){Id = 2}},
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
        Assert.NotEqual("Adam", result!.FirstName);
        Assert.NotEqual("Smith", result.LastName);
        Assert.Equal("ccc@gmail.c", result.Email);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetWorkingCleanersForDate_WhenCleanerIsWorking_ThenItAddedToList()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CleanersRepository(context);
        var orderDate = new DateTime(2022, 8, 1, 10, 00, 00);
        var cleaner1 = new Cleaner()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com1",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
            Schedule = Schedule.ShiftWork,
            Orders = new List<Order>(),
            DateOfStartWork = new DateTime(2022, 8, 1, 10, 00, 00),
            Passport = "0000654321"
        };

        var cleaner2 = new Cleaner()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com2",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
            Schedule = Schedule.FullTime,
            Orders = new List<Order>(),
            DateOfStartWork = new DateTime(2022, 8, 1, 10, 00, 00),
            Passport = "0000654321"
        };

        var cleaner3 = new Cleaner()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com3",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
            Schedule = Schedule.ShiftWork,
            Orders = new List<Order>(),
            DateOfStartWork = new DateTime(2022, 7, 20, 10, 00, 00),
            Passport = "0000654321"
        };
        context.Cleaners.Add(cleaner1);
        context.Cleaners.Add(cleaner2);
        context.Cleaners.Add(cleaner3);
        context.SaveChanges();

        var expectedCount = 3;

        //when
        var actual = await sut.GetWorkingCleanersForDate(orderDate);

        //then
        Assert.Equal(expectedCount, actual.Count);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetWorkingCleanersForDate_WhenCleanerIsNotWorking_ThenItDidNotAddToList()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CleanersRepository(context);
        var orderDate = new DateTime(2022, 8, 7, 10, 00, 00);
        var cleaner1 = new Cleaner()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com1",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
            Schedule = Schedule.ShiftWork,
            Orders = new List<Order>(),
            DateOfStartWork = new DateTime(2022, 8, 1, 10, 00, 00),
            Passport = "0000654321"
        };

        var cleaner2 = new Cleaner()
        {
            FirstName = "Adam",
            LastName = "Smith",
            Password = "12345678",
            Email = "AdamSmith@gmail.com2",
            Phone = "85559997264",
            BirthDate = DateTime.Today,
            Schedule = Schedule.FullTime,
            Orders = new List<Order>(),
            DateOfStartWork = new DateTime(2022, 8, 1, 10, 00, 00),
            Passport = "0000654321"
        };
        context.Cleaners.Add(cleaner1);
        context.Cleaners.Add(cleaner2);
        context.SaveChanges();

        var expectedCount = 0;

        //when
        var actual = await sut.GetWorkingCleanersForDate(orderDate);

        //then
        Assert.Equal(expectedCount, actual.Count);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetCommentsAboutCleanerTest_WhenCommentsExist_ThenGotIts()
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
        var comment4 = new Comment { Id = 4, Order = order1, Cleaner = cleaner, Rating = 2 };

        context.Cleaners.Add(cleaner);
        context.Clients.Add(client);
        context.Orders.Add(order1);
        context.Orders.Add(order2);
        context.Comments.Add(comment1);
        context.Comments.Add(comment2);
        context.Comments.Add(comment3);
        context.Comments.Add(comment4);
        context.SaveChanges();

        var expectedCount = 2;

        // when
        var result = await sut.GetCommentsAboutCleaner(cleaner.Id);

        // then
        Assert.Equal(expectedCount, result.Count);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetCommentsAboutCleanerTest_WhenCommentsDoNotExist_ThenListIsEmpty()
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

        var order1 = new Order { Id = 1, CleanersBand = cleanersBand };
        var order2 = new Order { Id = 2, CleanersBand = cleanersBand };
        var comment1 = new Comment { Id = 1, Order = order1, Cleaner = cleaner, Rating = 5 };
        var comment2 = new Comment { Id = 2, Order = order2, Cleaner = cleaner, Rating = 4 };

        context.Cleaners.Add(cleaner);
        context.Orders.Add(order1);
        context.Orders.Add(order2);
        context.Comments.Add(comment1);
        context.Comments.Add(comment2);
        context.SaveChanges();

        var expectedCount = 0;

        // when
        var result = await sut.GetCommentsAboutCleaner(cleaner.Id);

        // then
        Assert.Equal(expectedCount, result.Count);
        context.Database.EnsureDeleted();
    }
}
