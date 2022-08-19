using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.DataLayer.Tests;

public class OrdersRepositoryTests
{
    private readonly DbContextOptions<YogurtCleaningContext> _dbContextOptions;
    public OrdersRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<YogurtCleaningContext>()
            .UseInMemoryDatabase(databaseName: "TestDbForOrders")
            .Options;
        
    }

    [Fact]
    public async Task AddOrder_WhenOrderAdded_ThenOrderIdMoreThenZero()
    {
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new OrdersRepository(context);
        var order = new Order()
        {
            Id = 1,

            IsDeleted = false
        };

        // when
        var actual = await sut.CreateOrder(order);

        //then
        Assert.True(actual > 0);        
    }

    [Fact]
    public async Task DeleteOrder_WhenCorrectIdPassed_ThenSoftDeleteApplied()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new OrdersRepository(context);
        var order = new Order
        {
            Id = 1,
            Client = new() 
            { 
                Id = 1,
                FirstName = "Madara",
                LastName = "Smith",
                Email = "ychiha@gmail.japan",
                Password = "1234qwerty",
                Phone = "899988873456",
                IsDeleted = false
            },
            CleaningObject = new() 
            { 
                Id = 1,
                NumberOfRooms = 1000,
                NumberOfBathrooms = 1,
                Square = 1,
                NumberOfWindows = 1,
                NumberOfBalconies = 0,
                Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
                IsDeleted = false
            },
            CleanersBand = new() { new() 
            { 
                Id = 1,
                FirstName = "Adam",
                LastName = "Smith",
                Email = "ccc@gmail.c",
                Password = "1234qwerty",
                Passport = "0000654321",
                Phone = "89998887766",
                IsDeleted = false
            } },
            Bundles = new() { new()
            {
                Id = 1,
                Name = "qweqwe",
                Price = 2000,
                Measure = Measure.Room,
            } },
            Services = new() { new() 
            { 
                Id = 1,
                Name = "qwe",
                Price = 1000,
                Unit = "Unit",
                IsDeleted = false
            } },
            IsDeleted = false
        };

        context.Orders.Add(order);
        context.SaveChanges();

        // when
        await sut.DeleteOrder(order);

        //then
        var actual = await sut.GetOrder(order.Id);
        Assert.True(actual.IsDeleted);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task GetAllOrders_WhenOrdersExist_ThenGetOrders()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new OrdersRepository(context);
        var orders = new List<Order>()
        {
            new Order()
            {
                 Id = 4,
            Client = new()
            {
                Id = 4,
                FirstName = "Madara",
                LastName = "Smith",
                Email = "ychiha@gmail.japan",
                Password = "1234qwerty",
                Phone = "899988873456",
                IsDeleted = false
            },
            CleaningObject = new()
            {
                Id = 4,
                NumberOfRooms = 1000,
                NumberOfBathrooms = 1,
                Square = 1,
                NumberOfWindows = 1,
                NumberOfBalconies = 0,
                Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
                IsDeleted = false
            },
            CleanersBand = new() { new()
            {
                Id = 4,
                FirstName = "Adam",
                LastName = "Smith",
                Email = "ccc@gmail.c",
                Password = "1234qwerty",
                Passport = "0000654321",
                Phone = "89998887766",
                IsDeleted = false
            } },
            Bundles = new() { new()
            {
                Id = 4,
                Name = "qweqwe",
                Price = 2000,
                Measure = Measure.Room,
            } },
            Services = new() { new()
            {
                Id = 4,
                Name = "qwe",
                Price = 1000,
                Unit = "Unit",
                IsDeleted = false
            } },
                IsDeleted = true
            },
            new Order()
            {
                 Id = 3,
            Client = new()
            {
                Id = 3,
                FirstName = "Madara",
                LastName = "Smith",
                Email = "ychiha@gmail.japan",
                Password = "1234qwerty",
                Phone = "899988873456",
                IsDeleted = false
            },
            CleaningObject = new()
            {
                Id = 3,
                NumberOfRooms = 1000,
                NumberOfBathrooms = 1,
                Square = 1,
                NumberOfWindows = 1,
                NumberOfBalconies = 0,
                Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
                IsDeleted = false
            },
            CleanersBand = new() { new()
            {
                Id = 3,
                FirstName = "Adam",
                LastName = "Smith",
                Email = "ccc@gmail.c",
                Password = "1234qwerty",
                Passport = "0000654321",
                Phone = "89998887766",
                IsDeleted = false
            } },
            Bundles = new() { new()
            {
                Id = 3,
                Name = "qweqwe",
                Price = 2000,
                Measure = Measure.Room,
            } },
            Services = new() { new()
            {
                Id = 3,
                Name = "qwe",
                Price = 1000,
                Unit = "Unit",
                IsDeleted = false
            } },
            IsDeleted = false
            }
        };

        context.Orders.AddRange(orders);
        context.SaveChanges();

        // when
        var result = await sut.GetAllOrders();

        //then
        Assert.NotNull(result);
        Assert.Equal(typeof(List<Order>), result.GetType());
        Assert.True(result.Count() == 1);
        Assert.False(result[0].IsDeleted);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task UpdateOrder_WhenOrderUpdated_ThenOrderDoesNotHaveOldProperty()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new OrdersRepository(context);
        var oldOrder = new Order()
        {
            Id = 5,
            Client = new()
            {
                Id = 1,
                FirstName = "Madara",
                LastName = "Smith",
                Email = "ychiha@gmail.japan",
                Password = "1234qwerty",
                Phone = "899988873456",
                IsDeleted = false
            },
            CleaningObject = new()
            {
                Id = 1,
                NumberOfRooms = 1000,
                NumberOfBathrooms = 1,
                Square = 1,
                NumberOfWindows = 1,
                NumberOfBalconies = 0,
                Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
                IsDeleted = false
            },
            CleanersBand = new() { new()
            {
                Id = 1,
                FirstName = "Adam",
                LastName = "Smith",
                Email = "ccc@gmail.c",
                Password = "1234qwerty",
                Passport = "0000654321",
                Phone = "89998887766",
                IsDeleted = false
            } },
            Bundles = new() { new()
            {
                Id = 1,
                Name = "qweqwe",
                Price = 2000,
                Measure = Measure.Room,
            } },
            Services = new() { new()
            {
                Id = 1,
                Name = "qwe",
                Price = 1000,
                Unit = "Unit",
                IsDeleted = false
            } },
            Price = 15,
            EndTime = DateTime.Now,
            IsDeleted = false
        };
        context.Orders.Add(oldOrder);
        context.SaveChanges();
        oldOrder.Price = 10;
        oldOrder.EndTime = DateTime.MinValue;

        // when
        await sut.UpdateOrder(oldOrder);
        var result = await sut.GetOrder(oldOrder.Id);

        //then
        Assert.NotEqual(15, result.Price);
        Assert.Equal(result.EndTime, DateTime.MinValue);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public async Task UpdateOrderStatus_WhenOrderSatusChange_ThenOrderHasNotOldStatus()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new OrdersRepository(context);
        var oldOrder = new Order()
        {
            Id = 1,
            Client = new()
            {
                Id = 1,
                FirstName = "Madara",
                LastName = "Smith",
                Email = "ychiha@gmail.japan",
                Password = "1234qwerty",
                Phone = "899988873456",
                IsDeleted = false
            },
            CleaningObject = new()
            {
                Id = 1,
                NumberOfRooms = 1000,
                NumberOfBathrooms = 1,
                Square = 1,
                NumberOfWindows = 1,
                NumberOfBalconies = 0,
                Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
                IsDeleted = false
            },
            CleanersBand = new() { new()
            {
                Id = 1,
                FirstName = "Adam",
                LastName = "Smith",
                Email = "ccc@gmail.c",
                Password = "1234qwerty",
                Passport = "0000654321",
                Phone = "89998887766",
                IsDeleted = false
            } },
            Bundles = new() { new()
            {
                Id = 1,
                Name = "qweqwe",
                Price = 2000,
                Measure = Measure.Room,
            } },
            Services = new() { new()
            {
                Id = 1,
                Name = "qwe",
                Price = 1000,
                Unit = "Unit",
                IsDeleted = false
            } },
            Status = Status.Created,
            IsDeleted = false
        };
        context.Orders.Add(oldOrder);
        context.SaveChanges();

        // when
        await sut.UpdateOrderStatus(oldOrder.Id, Status.Done);
        var result = await sut.GetOrder(oldOrder.Id);

        //then
        Assert.NotEqual(Status.Created, result.Status);
        Assert.Equal(Status.Done, result.Status);
        context.Database.EnsureDeleted();
    }   
}