using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YogurtCleaning.Business;
using YogurtCleaning.Business.Services;
using YogurtCleaning.Controllers;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.API.Tests;

public class OrdersControllerTests
{
    private OrdersController _sut;
    private Mock<IOrdersService> _ordersServiceMock;
    private Mock<IOrdersRepository> _ordersRepositoryMock;
    private IMapper _mapper;
    private UserValues _userValues;

    [SetUp]
    public void Setup()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigStorage>()));
        _ordersServiceMock = new Mock<IOrdersService>();
        _ordersRepositoryMock = new Mock<IOrdersRepository>();
        _sut = new OrdersController(_ordersRepositoryMock.Object, _mapper, _ordersServiceMock.Object);
        _userValues = new UserValues();
    }

    [Test]
    public void DeleteOrderById_WhenValidRequestPassed_NoContentReceived()
    {
        //given
        var expectedOrder = new Order()
        {
            Id = 1,
            
            IsDeleted = false
        };

        _ordersRepositoryMock.Setup(o => o.GetOrder(expectedOrder.Id)).Returns(expectedOrder);

        //when
        var actual = _sut.DeleteOrder(expectedOrder.Id);

        //then
        var actualResult = actual as NoContentResult;

        Assert.AreEqual(StatusCodes.Status204NoContent, actualResult.StatusCode);
        _ordersServiceMock.Verify(c => c.DeleteOrder(expectedOrder.Id, It.IsAny<UserValues>()), Times.Once);
    }
}
