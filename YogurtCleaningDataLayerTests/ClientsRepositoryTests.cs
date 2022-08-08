using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.DataLayer.Tests;

public class ClientsRepositoryTests
{
    private readonly DbContextOptions<YogurtCleaningContext> _dbContextOptions;
    public ClientsRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<YogurtCleaningContext>()
            .UseInMemoryDatabase(databaseName: "TestDbForClients")
            .Options;
    }

    [Fact]
    public async Task AddClient_WhenClientAdded_ThenCommentIdMoreThenZero()
    {
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new ClientsRepository(context);
        var client = new Client
        {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Email = "ccc@gmail.c",
            Password = "1234qwerty",
            Phone = "89998887766",
            IsDeleted = false
        };

        // when
        var actual = await sut.CreateClient(client);

        //then
        Assert.True(actual > 0);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task DeleteClient_WhenCorrectIdPassed_ThenSoftDeleteApplied()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new ClientsRepository(context);
        var client = new Client
        {
            Id = 2,
            FirstName = "Adam",
            LastName = "Smith",
            Email = "ccc@gmail.c",
            Password = "1234qwerty",
            Phone = "89998887766",
            IsDeleted = false
        };

        context.Clients.Add(client);
        context.SaveChanges();

        // when
        await sut.DeleteClient(client);

        //then
        var actual = await sut.GetClient(client.Id);
        Assert.True(actual.IsDeleted);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllClients_WhenClientsExist_ThenGetClients()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new ClientsRepository(context);
        var clients = new List<Client>()
        {
            new Client()
            {
                Id = 3,
                FirstName = "Adam",
                LastName = "Smith",
                Email = "ccc@gmail.c",
                Password = "1234qwerty",
                Phone = "89998887766",
                IsDeleted = false
            },
            new Client()
            {
                Id = 4,
                FirstName = "Madara",
                LastName = "Smith",
                Email = "ychiha@gmail.japan",
                Password = "1234qwerty",
                Phone = "899988873456",
                IsDeleted = false
            }
        };

        context.Clients.AddRange(clients);
        context.SaveChanges();

        // when
        var result = await sut.GetAllClients();

        //then
        Assert.NotNull(result);
        Assert.Equal(typeof(List<Client>), result.GetType());
        Assert.Null(result[0].Comments);
        Assert.Null(result[1].Orders);
        Assert.Null(result[1].Addresses);
        Assert.False(result[0].IsDeleted);
        Assert.False(result[1].IsDeleted);
        Assert.NotNull(result.Find(x => x.FirstName == "Madara"));
        Assert.NotNull(result.Find(x => x.FirstName == "Adam"));
        Assert.Null(result.Find(x => x.FirstName == "Ilya"));
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllClients_WhenClientIsDeleted_ThenClientDoesNotGet()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new ClientsRepository(context);
        var clients = new List<Client>()
        {
            new Client()
            {
                Id = 5,
                FirstName = "Adam",
                LastName = "Smith",
                Email = "ccc@gmail.c",
                Password = "1234qwerty",
                Phone = "89998887766",
                IsDeleted = false
            },
            new Client()
            {
                Id = 6,
                FirstName = "Madara",
                LastName = "Smith",
                Email = "ychiha@gmail.japan",
                Password = "1234qwerty",
                Phone = "899988873456",
                IsDeleted = true
            }
        };

        context.Clients.AddRange(clients);
        context.SaveChanges();

        // when
        var result = await sut.GetAllClients();

        //then
        Assert.NotNull(result);
        Assert.Equal(typeof(List<Client>), result.GetType());
        Assert.True(result.Count == 1);
        Assert.Null(result[0].Comments);
        Assert.Null(result[0].Orders);
        Assert.Null(result[0].Addresses);
        Assert.False(result[0].IsDeleted);
        Assert.Null(result.Find(x => x.FirstName == "Madara"));
        Assert.NotNull(result.Find(x => x.FirstName == "Adam"));
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllCommentsByClient_WhenCommetsGet_ThenCommentsGet()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new ClientsRepository(context);
        var client = new Client
        {
            Id = 7,
            FirstName = "Adam",
            LastName = "Smith",
            Email = "ccc@gmail.c",
            Password = "1234qwerty",
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
        context.Clients.Add(client);
        context.SaveChanges();

        // when
        var result = await sut.GetAllCommentsByClient(client.Id);

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
    public async Task UpdateClient_WhenClientUpdated_ThenClientDoesNotHaveOldProperty()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new ClientsRepository(context);
        var client = new Client
        {
            Id = 7,
            FirstName = "Adam",
            LastName = "Smith",
            Email = "ccc@gmail.c",
            Password = "1234qwerty",
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
        context.Clients.Add(client);
        context.SaveChanges();
        client.FirstName = "Vasya";
        client.LastName = "Pupkin";

        //when
        await sut.UpdateClient(client);
        var result = await sut.GetClient(client.Id);

        //then
        Assert.NotEqual("Adam", result.FirstName);
        Assert.NotEqual("Smith", result.LastName);
        Assert.Equal("ccc@gmail.c", result.Email);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetLastOrderForCleaningObject_WhenOrderExist_ThenItRecieved()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new ClientsRepository(context);
        var client = new Client
        {
            Id = 7,
            FirstName = "Adam",
            LastName = "Smith",
            Email = "ccc@gmail.c",
            Password = "1234qwerty",
            Phone = "89998887766",
            IsDeleted = false,
            Comments = new List<Comment>(),
            Orders = new List<Order>()
            {
                new Order(){ Id = 1, StartTime = DateTime.Now, CleaningObject = new(){ Id = 1, Address = ""} },
            }
        };
        context.Clients.Add(client);
        context.SaveChanges();
        var expectedId = 1;

        // when
        var result = await sut.GetLastOrderForCleaningObject(client.Id, 1);

        //then
        Assert.NotNull(result);
        Assert.Equal(expectedId, result.Id);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetLastOrderForCleaningObject_WhenFewOrdersExist_ThenLastOneRecieved()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new ClientsRepository(context);
        var cleaningObject = new CleaningObject() { Id = 1, Address = "" };
        var client = new Client
        {
            Id = 7,
            FirstName = "Adam",
            LastName = "Smith",
            Email = "ccc@gmail.c",
            Password = "1234qwerty",
            Phone = "89998887766",
            IsDeleted = false,
            Comments = new List<Comment>(),
            Orders = new List<Order>()
            {
                new Order(){ Id = 1, StartTime = DateTime.Now, CleaningObject = cleaningObject },
                new Order(){ Id = 2, StartTime = DateTime.Now.AddDays(1), CleaningObject = cleaningObject },
            }
        };
        context.Clients.Add(client);
        context.SaveChanges();
        var expectedId = 2;

        // when
        var result = await sut.GetLastOrderForCleaningObject(client.Id, 1);

        //then
        Assert.NotNull(result);
        Assert.Equal(expectedId, result.Id);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetLastOrderForCleaningObject_WhenOrdersNotExist_ThenNullRecieved()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new ClientsRepository(context);
        var client = new Client
        {
            Id = 7,
            FirstName = "Adam",
            LastName = "Smith",
            Email = "ccc@gmail.c",
            Password = "1234qwerty",
            Phone = "89998887766",
            IsDeleted = false,
            Comments = new List<Comment>(),
            Orders = new List<Order>()
            {
                new Order(){ Id = 1, StartTime = DateTime.Now, CleaningObject = new CleaningObject() { Id = 2, Address = "" } },
                new Order(){ Id = 2, StartTime = DateTime.Now.AddDays(1), CleaningObject = new CleaningObject() { Id = 3, Address = "" } },
            }
        };
        context.Clients.Add(client);
        context.SaveChanges();

        // when
        var result = await sut.GetLastOrderForCleaningObject(client.Id, 1);

        //then
        Assert.Null(result);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task UpdateClientRatingTest_WhenValidRequestPassed_ThenRatingUpdated()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new ClientsRepository(context);

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
        var order1 = new Order { Id = 1, CleanersBand = cleanersBand, Client = client };
        var order2 = new Order { Id = 2, CleanersBand = cleanersBand, Client = client };
        var comment1 = new Comment { Id = 1, Order = order1, Cleaner = cleaner, Rating = 2 };
        var comment2 = new Comment { Id = 2, Order = order2, Client = client, Rating = 1 };
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

        var expectedRating = 3.5m;

        // when
        await sut.UpdateClientRating(client.Id);
        var result = await sut.GetClient(client.Id);

        // then
        Assert.Equal(expectedRating, result.Rating);
        context.Database.EnsureDeleted();
    }
}