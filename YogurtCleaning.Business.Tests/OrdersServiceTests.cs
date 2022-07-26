using Moq;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
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
        _ordersRepositoryMock.Setup(o => o.DeleteOrder(expectedOrder.Id));
        userValue = new UserValues() { Email = "AdamSmith@gmail.com3", Role = "Admin", Id = 1 };

        //when
        _sut.DeleteOrder(expectedOrder.Id, userValue);

        //then
        _ordersRepositoryMock.Verify(c => c.DeleteOrder(expectedOrder.Id), Times.Once);
    }

    [Fact]
    public void DeleteOrder_EmptyOrderRequest_ThrowEntityNotFoundException()
    {
        //given
        var testId = 1;
        var order = new Order();
        var testEmail = "FakeOrder@gmail.ru";
        userValue = new UserValues() { Email = testEmail, Role = "Admin" };
        _ordersRepositoryMock.Setup(o => o.DeleteOrder(testId));

        //when

        //then
        Assert.Throws<Exceptions.BadRequestException>(() => _sut.DeleteOrder(testId, userValue));
    }
}