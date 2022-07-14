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
    }

    [Fact]
    public void GetAllClients_WhenClientsExist_ThenGetClients()
    {
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
    }

    [Fact]
    public void GetAllClients_WhenClientIsDeleted_ThenClientDoesNotGet()
    {
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
    }

    [Fact]
    public void GetAllCommentsByClient_WhenCommetsGet_ThenCommentsGet()
    {
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
    }
}