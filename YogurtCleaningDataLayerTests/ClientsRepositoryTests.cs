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
    public void AddClient_WhenClientAdded_ThenCommentIdMoreThenZero()
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
        var actual = sut.CreateClient(client);

        //then
        Assert.True(actual > 0);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public void DeleteClient_WhenCorrectIdPassed_ThenSoftDeleteApplied()
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
        sut.DeleteClient(client.Id);

        //then
        var actual = sut.GetClient(client.Id);
        Assert.True(actual.IsDeleted);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public void GetAllClients_WhenClientsExist_ThenGetClients()
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
        var result = sut.GetAllClients();

        //then
        Assert.NotNull(result);
        Assert.True(result.GetType() == typeof(List<Client>));
        Assert.Null(result[0].Comments);
        Assert.Null(result[1].Orders);
        Assert.Null(result[1].Addresses);
        Assert.True(result[0].IsDeleted == false);
        Assert.True(result[1].IsDeleted == false);
        Assert.NotNull(result.Find(x => x.FirstName == "Madara"));
        Assert.NotNull(result.Find(x => x.FirstName == "Adam"));
        Assert.Null(result.Find(x => x.FirstName == "Ilya"));
        context.Database.EnsureDeleted();
    }

    [Fact]
    public void GetAllClients_WhenClientIsDeleted_ThenClientDoesNotGet()
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
        var result = sut.GetAllClients();

        //then
        Assert.NotNull(result);
        Assert.True(result.GetType() == typeof(List<Client>));
        Assert.True(result.Count == 1);
        Assert.Null(result[0].Comments);
        Assert.Null(result[0].Orders);
        Assert.Null(result[0].Addresses);
        Assert.True(result[0].IsDeleted == false);
        Assert.Null(result.Find(x => x.FirstName == "Madara"));
        Assert.NotNull(result.Find(x => x.FirstName == "Adam"));
        context.Database.EnsureDeleted();
    }

    [Fact]
    public void GetAllCommentsByClient_WhenCommetsGet_ThenCommentsGet()
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
        var result = sut.GetAllCommentsByClient(client.Id);

        //then
        Assert.NotNull(result);
        Assert.True(result.GetType() == typeof(List<Comment>));
        Assert.True(result[0].IsDeleted == true);
        Assert.True(result[1].IsDeleted == false);
        Assert.NotNull(result.Find(x => x.Rating == 5));
        Assert.NotNull(result.Find(x => x.Order.Id == 1));
        context.Database.EnsureDeleted();
    }

    [Fact]
    public void UpdateClient_WhenClientUpdated_ThenClientDoesNotHaveOldProperty()
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
        sut.UpdateClient(client);
        var result = sut.GetClient(client.Id);

        //then
        Assert.False(result.FirstName == "Adam");
        Assert.False(result.LastName == "Smith");
        Assert.True(result.Email == "ccc@gmail.c");
        context.Database.EnsureDeleted();
    }

    [Fact]
    public void GetLastOrderForCleaningObject_WhenOrderExist_ThenItRecieved()
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
        var result = sut.GetLastOrderForCleaningObject(client.Id, 1);

        //then
        Assert.NotNull(result);
        Assert.Equal(expectedId, result.Id);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public void GetLastOrderForCleaningObject_WhenFewOrdersExist_ThenLastOneRecieved()
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
        var result = sut.GetLastOrderForCleaningObject(client.Id, 1);

        //then
        Assert.NotNull(result);
        Assert.Equal(expectedId, result.Id);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public void GetLastOrderForCleaningObject_WhenOrdersNotExist_ThenNullRecieved()
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
        var result = sut.GetLastOrderForCleaningObject(client.Id, 1);

        //then
        Assert.Null(result);
        context.Database.EnsureDeleted();
    }
}