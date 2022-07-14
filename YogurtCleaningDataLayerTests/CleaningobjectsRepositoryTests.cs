using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
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
    public void AddCleaningObject_WhenCleaningObjectAdded_ThenCleaningObjectIdMoreThenZero()
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
        var actual = sut.CreateCleaningObject(cleaningObject);

        //then
        Assert.True(actual > 0);
    }

    [Fact]
    public void DeleteCleaningObject_WhenCorrectIdPassed_ThenSoftDeleteApplied()
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
            IsDeleted = false
        };

        context.CleaningObjects.Add(cleaningObject);
        context.SaveChanges();

        // when
        sut.DeleteCleaningObject(cleaningObject.Id);

        //then
        var actual = sut.GetCleaningObject(cleaningObject.Id);
        Assert.True(actual.IsDeleted);
    }

    [Fact]
    public void GetAllCleaningObjects_WhenCleaningObjectsExist_ThenGetCleaningObjects()
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
                IsDeleted = false
            }
        };

        context.CleaningObjects.AddRange(cleaningObjects);
        context.SaveChanges();

        // when
        var result = sut.GetAllCleaningObjects();

        //then
        Assert.NotNull(result);
        Assert.True(result.GetType() == typeof(List<CleaningObject>));
        Assert.True(result[0].IsDeleted == false);
        Assert.Null(result.Find(x => x.NumberOfBalconies == 0));
        Assert.NotNull(result.Find(x => x.NumberOfRooms == 1000));
    }
}

//    [Fact]
//    public void GetAllCleaningObjects_WhenCleaningObjectIsDeleted_ThenCleaningObjectDoesNotGet()
//    {
//        var context = new YogurtCleaningContext(_dbContextOptions);
//        var sut = new CleaningObjectsRepository(context);
//        var cleaningObject = new CleaningObject
//        {
//            Rating = 1,
//            Client = new()
//            {
//                Id = 4,
//                Email = "b@b.c",
//                FirstName = "q",
//                LastName = "w",
//                Password = "qweqweqweqwe43",
//                Phone = "19998887766"
//            },
//            Order = new() { Id = 4 },
//            IsDeleted = true
//        };

//        context.CleaningObjects.Add(cleaningObject);
//        context.SaveChanges();

//        // when
//        var result = sut.GetAllCleaningObjects();

//        //then
//        Assert.DoesNotContain(cleaningObject, result);
//    }
//}