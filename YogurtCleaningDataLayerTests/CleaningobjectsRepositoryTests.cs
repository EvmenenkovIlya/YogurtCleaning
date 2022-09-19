using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.DataLayer.Tests;

public class CleaningObjectsRepositoryTests
{
    private readonly DbContextOptions<YogurtCleaningContext> _dbContextOptions;
    public CleaningObjectsRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<YogurtCleaningContext>()
            .UseInMemoryDatabase(databaseName: "TestDbForCleaningObjects")
            .Options;
    }

    [Fact]
    public async Task AddCleaningObject_WhenCleaningObjectAdded_ThenCleaningObjectIdMoreThenZero()
    {
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CleaningObjectsRepository(context);
        var cleaningObject = new CleaningObject()
        {
            Id = 1,
            Client = new Client()
            {
                Id = 1,
                FirstName = "Adam",
                LastName = "Smith",
                Email = "ccc@gmail.c",
                Password = "1234qwerty",
                Phone = "89998887766",
                IsDeleted = false
            },
            NumberOfRooms = 1000,
            NumberOfBathrooms = 1,
            Square = 1,
            NumberOfWindows = 1,
            NumberOfBalconies = 0,
            Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
            IsDeleted = false
        };

        // when
        var actual = await sut.CreateCleaningObject(cleaningObject);

        //then
        Assert.True(actual > 0);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task DeleteCleaningObject_WhenCorrectIdPassed_ThenSoftDeleteApplied()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CleaningObjectsRepository(context);
        var cleaningObject = new CleaningObject
        {
            Id = 2,
            Client = new Client()
            {
                Id = 2,
                FirstName = "Adam",
                LastName = "Smith",
                Email = "ccc@gmail.c",
                Password = "1234qwerty",
                Phone = "89998887766",
                IsDeleted = false
            },
            NumberOfRooms = 1000,
            NumberOfBathrooms = 1,
            Square = 1,
            NumberOfWindows = 1,
            NumberOfBalconies = 0,
            Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
            District = new District() { Id = DistrictEnum.Vasileostrovskiy, Name = DistrictEnum.Vasileostrovskiy.ToString()},
            IsDeleted = false
        };

        context.CleaningObjects.Add(cleaningObject);
        context.SaveChanges();

        // when
        await sut.DeleteCleaningObject(cleaningObject);

        //then
        var actual = await sut.GetCleaningObject(cleaningObject.Id);
        Assert.True(actual.IsDeleted);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllCleaningObjects_WhenCleaningObjectsExist_ThenGetCleaningObjects()
    {
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CleaningObjectsRepository(context);
        var cleaningObjects = new List<CleaningObject>()
        {
            new CleaningObject()
            {
                Id = 3,
                Client = new Client()
                {
                    Id = 3,
                    FirstName = "Adam",
                    LastName = "Smith",
                    Email = "ccc@gmail.c",
                    Password = "1234qwerty",
                    Phone = "89998887766",
                    IsDeleted = false
                },
                NumberOfRooms = 1000,
                NumberOfBathrooms = 1,
                Square = 1,
                NumberOfWindows = 1,
                NumberOfBalconies = 12,
                Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
                District = new District() { Id = DistrictEnum.Vasileostrovskiy, Name = DistrictEnum.Vasileostrovskiy.ToString()},
                IsDeleted = true
            },
            new CleaningObject()
            {
                Id = 4,
                Client = new Client()
                {
                    Id = 4,
                    FirstName = "Adam",
                    LastName = "Smith",
                    Email = "ccc@gmail.c",
                    Password = "1234qwerty",
                    Phone = "89998887766",
                    IsDeleted = false
                },
                NumberOfRooms = 10,
                NumberOfBathrooms = 1,
                Square = 1,
                NumberOfWindows = 1,
                NumberOfBalconies = 0,
                Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
                District = new District() { Id = DistrictEnum.Admiralteisky, Name = DistrictEnum.Admiralteisky.ToString()},
                IsDeleted = false
            }
        };

        context.CleaningObjects.AddRange(cleaningObjects);
        context.SaveChanges();

        // when
        var result = await sut.GetAllCleaningObjects();

        //then
        Assert.NotNull(result);
        Assert.Equal(typeof(List<CleaningObject>), result.GetType());
        Assert.False(result[0].IsDeleted);
        Assert.NotNull(result.Find(x => x.NumberOfBalconies == 0));
        Assert.NotNull(result.Find(x => x.NumberOfRooms == 10));
        Assert.Null(result.Find(x => x.NumberOfBalconies == 12));
        Assert.Null(result.Find(x => x.NumberOfRooms == 1000));
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task UpdateCleaningObject_WhenCleaningObjectUpdated_ThenCleaningObjectDoesNotHaveOldProperty()
    {
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CleaningObjectsRepository(context);
        var oldCleaningObject = new CleaningObject()
        {
            Id = 5,
            Client = new Client()
            {
                Id = 5,
                FirstName = "Adam",
                LastName = "Smith",
                Email = "ccc@gmail.c",
                Password = "1234qwerty",
                Phone = "89998887766",
                IsDeleted = false
            },
            NumberOfRooms = 1000,
            NumberOfBathrooms = 1,
            Square = 1,
            NumberOfWindows = 1,
            NumberOfBalconies = 12,
            Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
            District = new District() { Id = DistrictEnum.Vasileostrovskiy, Name = DistrictEnum.Vasileostrovskiy.ToString() },
            IsDeleted = false
        };        
        context.CleaningObjects.Add(oldCleaningObject);
        context.SaveChanges();
        oldCleaningObject.Address = "г. Москва, ул. Льва Толстого, д. 19, кв. 8";
        oldCleaningObject.NumberOfRooms = 10;
        oldCleaningObject.NumberOfBathrooms = 10;

        // when
        await sut.UpdateCleaningObject(oldCleaningObject);
        var result = await sut.GetCleaningObject(oldCleaningObject.Id);

        //then
        Assert.NotEqual(1, result.NumberOfBathrooms);
        Assert.NotEqual(1000, result.NumberOfRooms);
        Assert.Equal(1, result.Square);
        Assert.NotEqual("г. Санкт - Петербург, ул. Льва Толстого, д. 16, кв. 10", result.Address);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllCleaningObjectsByClient_WhenCleaningObjectsExist_ThenGetCleaningObjects()
    {
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new CleaningObjectsRepository(context);
        var clients = new List<Client>() 
        {
            new Client()
            {
            Id = 1,
            FirstName = "Adam",
            LastName = "Smith",
            Email = "ccc@gmail.c",
            Password = "1234qwerty",
            Phone = "89998887766",
            IsDeleted = false
            },
            new Client()
            {
                    Id = 2,
                    FirstName = "Adam",
                    LastName = "Smith",
                    Email = "ccc@gmail.c",
                    Password = "1234qwerty",
                    Phone = "89998887766",
                    IsDeleted = false
             } 
        };
        var cleaningObjects = new List<CleaningObject>()
        {
            new CleaningObject()
            {
                Id = 1,
                Client = clients[0],
                NumberOfRooms = 1000,
                NumberOfBathrooms = 1,
                Square = 1,
                NumberOfWindows = 1,
                NumberOfBalconies = 12,
                Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
                District = new District(){ Id = Enums.DistrictEnum.Kronstadt, Name = "Кронштадт"},
                IsDeleted = false
            },
            new CleaningObject()
            {
                Id = 2,
                Client = clients[1],
                NumberOfRooms = 10,
                Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 15",
                District = new District(){ Id = Enums.DistrictEnum.Krasnoselsky, Name = "Красносельский"},
                IsDeleted = false
            },
            new CleaningObject()
            {
                Id = 3,
                Client = clients[0],
                NumberOfRooms = 10,
                Address = "г. Питер, ул. Льва Толстого, д. 16, кв. 10",
                District = new District(){ Id = Enums.DistrictEnum.Vasileostrovskiy, Name = "Василеостровский"},
                IsDeleted = false
            },
        };
        context.Clients.AddRange(clients);
        context.CleaningObjects.AddRange(cleaningObjects);
        context.SaveChanges();

        // when
        var result = await sut.GetAllCleaningObjectsByClientId(clients[0].Id);

        //then
        Assert.NotNull(result);
        Assert.Equal(typeof(List<CleaningObject>), result.GetType());
        Assert.Equal(2, result.Count());
        context.Database.EnsureDeleted();
    }
}