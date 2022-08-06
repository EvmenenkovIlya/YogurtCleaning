using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
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
    public void AddOrder_WhenOrderAdded_ThenOrderIdMoreThenZero()
    {
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new OrdersRepository(context);
        var order = new Order()
        {
            Id = 1,

            IsDeleted = false
        };

        // when
        var actual = sut.CreateOrder(order);

        //then
        Assert.True(actual > 0);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public void DeleteOrder_WhenCorrectIdPassed_ThenSoftDeleteApplied()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new OrdersRepository(context);
        var order = new Order
        {
            Id = 1,

            IsDeleted = false
        };

        context.Orders.Add(order);
        context.SaveChanges();

        // when
        sut.DeleteOrder(order);

        //then
        var actual = sut.GetOrder(order.Id);
        Assert.True(actual.IsDeleted);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public void GetAllOrders_WhenOrdersExist_ThenGetOrders()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new OrdersRepository(context);
        var orders = new List<Order>()
        {
            new Order()
            {
                Id = 1,
                IsDeleted = true
            },
            new Order()
            {
                Id = 2,
                IsDeleted = false
            }
        };

        context.Orders.AddRange(orders);
        context.SaveChanges();

        // when
        var result = sut.GetAllOrders();

        //then
        Assert.NotNull(result);
        Assert.Equal(typeof(List<Order>), result.GetType());
        Assert.True(result.Count() == 1);
        Assert.False(result[0].IsDeleted);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public void UpdateOrder_WhenOrderUpdated_ThenOrderDoesNotHaveOldProperty()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new OrdersRepository(context);
        var oldOrder = new Order()
        {
            Id = 1,
            Price = 15,
            EndTime = DateTime.Now,
            IsDeleted = false
        };
        context.Orders.Add(oldOrder);
        context.SaveChanges();
        oldOrder.Price = 10;
        oldOrder.EndTime = DateTime.MinValue;

        // when
        sut.UpdateOrder(oldOrder);
        var result = sut.GetOrder(oldOrder.Id);

        //then
        Assert.NotEqual(15, result.Price);
        Assert.Equal(result.EndTime, DateTime.MinValue);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public void GetAllServicesByOrder_WhenOrderHasServices_ThenGetAllServicesByOrder()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new OrdersRepository(context);
        var orders = new List<Order>()
        {
            new Order()
            {
                Id = 1,
                Services = new List<Service>()
                {
                    new Service()
                    {
                        Id = 1,
                        Name = "Wash Tables",
                        Unit = "unit",
                        IsDeleted = false
                    },
                    new Service()
                    {
                        Id = 2,
                        Name = "Wash floor",
                        Unit = "unit",
                        IsDeleted = false
                    }
                },
                IsDeleted = false
            },
            new Order()
            {
                Id = 2,
                Services = new List<Service>()
                {
                    new Service()
                    {
                        Id = 3,
                        Name = "Wash wall",
                        Unit = "unit",
                        IsDeleted = false
                    }
                },
                IsDeleted = false
            }
        };
        context.Orders.AddRange(orders);
        context.SaveChanges();

        // when
        var result = sut.GetServices(orders[0].Id);

        //then
        Assert.NotNull(result);
        Assert.Equal(typeof(List<Service>), result.GetType());
        Assert.True(result.Count() == 2);
        Assert.False(result[0].IsDeleted);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public void UpdateOrderStatus_WhenOrderSatusChange_ThenOrderHasNotOldStatus()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new OrdersRepository(context);
        var oldOrder = new Order()
        {
            Id = 1,
            Price = 15,
            EndTime = DateTime.Now,
            Status = Enums.Status.Created,
            IsDeleted = false
        };
        context.Orders.Add(oldOrder);
        context.SaveChanges();

        // when
        sut.UpdateOrderStatus(oldOrder.Id, Enums.Status.Done);
        var result = sut.GetOrder(oldOrder.Id);

        //then
        Assert.NotEqual(Enums.Status.Created, result.Status);
        Assert.Equal(Enums.Status.Done, result.Status);
        context.Database.EnsureDeleted();
    }

    [Fact]
    public void GetCleaningObject()
    {
        // given
        var context = new YogurtCleaningContext(_dbContextOptions);
        var sut = new OrdersRepository(context);
        var oldOrder = new Order()
        {
            Id = 1,
            Price = 15,
            EndTime = DateTime.Now,
            CleaningObject = new CleaningObject()
            {
                Id = 1,
                Address = "Улица Пушкина, 10",
                Square = 800,
                IsDeleted = false
            },
            Status = Enums.Status.Created,
            IsDeleted = false
        };
        context.Orders.Add(oldOrder);
        context.SaveChanges();

        // when
        var result = sut.GetCleaningObject(oldOrder.Id);

        //then
        Assert.Equal(800, result.Square);
        Assert.Equal("Улица Пушкина, 10", result.Address);
        context.Database.EnsureDeleted();
    }
}