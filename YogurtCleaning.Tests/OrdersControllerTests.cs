using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YogurtCleaning.Business;
using YogurtCleaning.Business.Services;
using YogurtCleaning.Controllers;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.Models;

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
    public async Task DeleteOrderById_WhenValidRequestPassed_NoContentReceived()
    {
        //given
        var expectedOrder = new Order()
        {
            Id = 1,
            
            IsDeleted = false
        };

        _ordersRepositoryMock.Setup(o => o.GetOrder(expectedOrder.Id)).ReturnsAsync(expectedOrder);

        //when
        var actual = _sut.DeleteOrder(expectedOrder.Id);

        //then
        var actualResult = actual as NoContentResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        _ordersServiceMock.Verify(c => c.DeleteOrder(expectedOrder.Id, It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public void GetAllOrders_WhenValidRequestPassed_RequestedTypeReceived()
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
            },
        };

        _ordersServiceMock.Setup(o => o.GetAllOrders()).Returns(orders).Verifiable();

        //when
        var actual = _sut.GetAllOrders();

        //then
        var actualResult = actual.Result as ObjectResult;
        var ordersResponse = actualResult.Value as List<OrderResponse>;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(ordersResponse.Count, Is.EqualTo(orders.Count));
        Assert.Multiple(() =>
        {
            Assert.That(ordersResponse[0].Client.Id, Is.EqualTo(orders[0].Client.Id));
            Assert.That(ordersResponse[0].CleaningObject.Id, Is.EqualTo(orders[0].CleaningObject.Id));
            Assert.That(ordersResponse[1].Price, Is.EqualTo(orders[1].Price));
            Assert.That(ordersResponse[1].CleanersBand.Count, Is.EqualTo(orders[1].CleanersBand.Count));
            Assert.That(ordersResponse[0].Comments.Count, Is.EqualTo(orders[0].Comments.Count));
            Assert.That(ordersResponse[0].Bundles.Count, Is.EqualTo(orders[0].Bundles.Count));
            Assert.That(ordersResponse[0].Services.Count, Is.EqualTo(orders[0].Services.Count));
            Assert.That(ordersResponse[0].Price, Is.EqualTo(orders[0].Price));
            Assert.That(ordersResponse[0].StartTime, Is.EqualTo(orders[0].StartTime));
            Assert.That(ordersResponse[0].EndTime, Is.EqualTo(orders[0].EndTime));
            Assert.That(ordersResponse[0].UpdateTime, Is.EqualTo(orders[0].UpdateTime));
            Assert.That(ordersResponse[0].Status, Is.EqualTo(orders[0].Status));
        });
        _ordersServiceMock.Verify(x => x.GetAllOrders(), Times.Once);
    }
}
