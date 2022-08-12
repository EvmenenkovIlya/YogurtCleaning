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
        context.Database.EnsureDeleted();
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
            Price = 15,
            EndTime = DateTime.Now,
            Status = Enums.Status.Created,
            IsDeleted = false
        };
        context.Orders.Add(oldOrder);
        context.SaveChanges();

        // when
        await sut.UpdateOrderStatus(oldOrder.Id, Enums.Status.Done);
        var result = await sut.GetOrder(oldOrder.Id);

        //then
        Assert.NotEqual(Enums.Status.Created, result.Status);
        Assert.Equal(Enums.Status.Done, result.Status);
        context.Database.EnsureDeleted();
    }   
}