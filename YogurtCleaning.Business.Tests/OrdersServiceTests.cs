using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Facts;

public class OrdersServiceFacts
{
    private OrdersService _sut;
    private Mock<IOrdersRepository> _ordersRepositoryMock;

    private UserValues userValue;

    public OrdersServiceFacts()
    {
        _ordersRepositoryMock = new Mock<IOrdersRepository>();
        _sut = new OrdersService(_ordersRepositoryMock.Object);
    }

    [Fact]
    public void DeleteOrder_WhenValidRequestPassed_DeleteOrder()
    {
        //given
        var expectedOrder = new Order()
        {
            Id = 1,
            Client = new Client() { Id = 3 },
            IsDeleted = false
        };

        _ordersRepositoryMock.Setup(o => o.GetOrder(expectedOrder.Id)).Returns(expectedOrder);
        _ordersRepositoryMock.Setup(o => o.DeleteOrder(expectedOrder));
        userValue = new UserValues() { Email = "AdamSmith@gmail.com3", Role = Role.Admin, Id = 1 };

        //when
        _sut.DeleteOrder(expectedOrder.Id, userValue);

        //then
        _ordersRepositoryMock.Verify(c => c.DeleteOrder(expectedOrder), Times.Once);
    }

    [Fact]
    public void DeleteOrder_EmptyOrderRequest_ThrowEntityNotFoundException()
    {
        //given
        var testId = 1;
        var order = new Order();
        var testEmail = "FakeOrder@gmail.ru";
        userValue = new UserValues() { Email = testEmail, Role = Role.Admin };

        //when

        //then
        Assert.Throws<Exceptions.BadRequestException>(() => _sut.DeleteOrder(testId, userValue));
    }

    [Fact]
    public void GetAllOrders_WhenValidRequestPassed_OrdersReceived()
    {
        //given
        var orders = new List<Order>
        {
            new Order()
            {
                Id = 1,
                Client = new Client() { Id = 1 },
                CleanersBand = new List<Cleaner>() { new Cleaner(){ Id = 1}, new Cleaner(){ Id = 2} },
                CleaningObject = new CleaningObject() {Id = 1},
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Price = 20,
                Bundles = new List<Bundle>() { new Bundle(){ Id = 1}, new Bundle(){ Id = 2} },
                Services = new List<Service>() { new Service(){ Id = 1}, new Service(){ Id = 2} },
                Comments = new List<Comment>() { new Comment(){ Id = 1}, new Comment(){ Id = 2} },
                Status = Status.Created,
                IsDeleted=false
            },
            new Order()
            {
                Id = 2,
                Client = new Client() { Id = 2 },
                CleanersBand = new List<Cleaner>() { new Cleaner(){ Id = 3}, new Cleaner(){ Id = 4} },
                CleaningObject = new CleaningObject() {Id = 2},
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Price = 30,
                IsDeleted=false
            }
        };
        _ordersRepositoryMock.Setup(o => o.GetAllOrders()).Returns(orders);

        //when
        var actual = _sut.GetAllOrders();

        //then
        Assert.NotNull(actual);
        Assert.Equal(orders.Count, actual.Count);
        _ordersRepositoryMock.Verify(c => c.GetAllOrders(), Times.Once);
    }
}