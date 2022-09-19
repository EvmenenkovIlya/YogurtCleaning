using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YogurtCleaning.Business;
using YogurtCleaning.Business.Models;
using YogurtCleaning.Business.Services;
using YogurtCleaning.Controllers;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Models;

namespace YogurtCleaning.API.Tests;

public class OrdersControllerTests
{
    private OrdersController _sut;
    private Mock<IOrdersService> _ordersServiceMock;

    private IMapper _mapper;
    private UserValues _userValues;

    [SetUp]
    public void Setup()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MapperConfigStorage>()));
        _ordersServiceMock = new Mock<IOrdersService>();
        _sut = new OrdersController(_mapper, _ordersServiceMock.Object);
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

        //when
        var actual = await _sut.DeleteOrder(expectedOrder.Id);

        //then
        var actualResult = actual as NoContentResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        _ordersServiceMock.Verify(c => c.DeleteOrder(expectedOrder.Id, It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public async Task GetAllOrders_WhenValidRequestPassed_RequestedTypeReceived()
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

        _ordersServiceMock.Setup(o => o.GetAllOrders()).ReturnsAsync(orders).Verifiable();

        //when
        var actual = await _sut.GetAllOrders();

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

    [Test]
    public async Task GetCleaningObject_WhenValidRequestPassed_OkReceived()
    {
        //given
        var expectedCleaningObject = new CleaningObject()
        {
            Id = 1,
            NumberOfRooms = 1000,
            NumberOfBathrooms = 1,
            Square = 1,
            NumberOfWindows = 1,
            NumberOfBalconies = 0,
            Address = "г. Москва, ул. Льва Толстого, д. 16, кв. 10",
        };

        _ordersServiceMock.Setup(o => o.GetCleaningObject(expectedCleaningObject.Id, It.IsAny<UserValues>())).ReturnsAsync(expectedCleaningObject);

        //when
        var actual = await _sut.GetCleaningObject(expectedCleaningObject.Id);

        //then

        var actualResult = actual.Result as ObjectResult;
        var cleaningObjectResponse = actualResult.Value as CleaningObjectResponse;
        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.Multiple(() =>
        {
            Assert.That(cleaningObjectResponse.NumberOfRooms, Is.EqualTo(expectedCleaningObject.NumberOfRooms));
            Assert.That(cleaningObjectResponse.NumberOfBathrooms, Is.EqualTo(expectedCleaningObject.NumberOfBathrooms));
            Assert.That(cleaningObjectResponse.Square, Is.EqualTo(expectedCleaningObject.Square));
            Assert.That(cleaningObjectResponse.NumberOfWindows, Is.EqualTo(expectedCleaningObject.NumberOfWindows));
            Assert.That(cleaningObjectResponse.Address, Is.EqualTo(expectedCleaningObject.Address));
        });
        _ordersServiceMock.Verify(x => x.GetCleaningObject(expectedCleaningObject.Id, It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public async Task GetOrder_WhenValidRequestPassed_OkReceived()
    {
        //given
        var expectedOrder = new Order()
        {
            Id = 1,
            Client = new Client() { Id = 1 },
            CleanersBand = new List<Cleaner>() { new Cleaner() { Id = 1 }, new Cleaner() { Id = 2 } },
            CleaningObject = new CleaningObject() { Id = 1 },
            StartTime = DateTime.Now,
            EndTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            Price = 20,
            Bundles = new List<Bundle>() { new Bundle() { Id = 1 }, new Bundle() { Id = 2 } },
            Services = new List<Service>() { new Service() { Id = 1 }, new Service() { Id = 2 } },
            Comments = new List<Comment>() { new Comment() { Id = 1 }, new Comment() { Id = 2 } },
            Status = Status.Created,
            IsDeleted = false
        };

        _ordersServiceMock.Setup(o => o.GetOrder(expectedOrder.Id, It.IsAny<UserValues>())).ReturnsAsync(expectedOrder);

        //when
        var actual = await _sut.GetOrder(expectedOrder.Id);

        //then
        var actualResult = actual.Result as ObjectResult;
        var orderResponse = actualResult.Value as OrderResponse;
        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.Multiple(() =>
        {
            Assert.That(orderResponse.Client.Id, Is.EqualTo(expectedOrder.Client.Id));
            Assert.That(orderResponse.CleaningObject.Id, Is.EqualTo(expectedOrder.CleaningObject.Id));
            Assert.That(orderResponse.Bundles.Count, Is.EqualTo(expectedOrder.Bundles.Count));
            Assert.That(orderResponse.Services.Count, Is.EqualTo(expectedOrder.Services.Count));
            Assert.That(orderResponse.Price, Is.EqualTo(expectedOrder.Price));
            Assert.That(orderResponse.StartTime, Is.EqualTo(expectedOrder.StartTime));
            Assert.That(orderResponse.EndTime, Is.EqualTo(expectedOrder.EndTime));
            Assert.That(orderResponse.UpdateTime, Is.EqualTo(expectedOrder.UpdateTime));
            Assert.That(orderResponse.Status, Is.EqualTo(expectedOrder.Status));
        });
        _ordersServiceMock.Verify(x => x.GetOrder(expectedOrder.Id, It.IsAny<UserValues>()), Times.Once);

    }

    [Test]
    public async Task UpdateOrder_WhenValidRequestPassed_NoContentReceived()
    {
        //given
        var order = new Order()
        {
            Id = 1,
        };
        var newOrderModel = new OrderUpdateRequest()
        {
            StartTime = DateTime.Now,
            ServicesIds = new List<int>() { 1, 2, 3 },
            BundlesIds = new List<int>() { 1, 2, 3 },
            CleanersBandIds = new List<int>() { 1, 2, 3 },
        };

    //when
    var actual = await _sut.UpdateOrder(newOrderModel, order.Id);

        //then
        var actualResult = actual as NoContentResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        _ordersServiceMock.Verify(c => c.UpdateOrder(It.Is<OrderBusinessModel>(c =>
        c.StartTime == newOrderModel.StartTime &&
        c.Services.Count == newOrderModel.ServicesIds.Count &&
        c.Bundles.Count == newOrderModel.BundlesIds.Count &&
        c.CleanersBand.Count == newOrderModel.CleanersBandIds.Count
        ), It.Is<int>(i => i == order.Id), It.IsAny<UserValues>()), Times.Once);
    }

    [Test]
    public async Task UpdateOrderStatus_WhenValidRequestPassed_NoContentReceived()
    {
        //given
        var order = new Order()
        {
            Id = 1,
        };
        var status = Status.Done;

        //when
        var actual = await _sut.UpdateOrderStatus(order.Id, status);

        //then
        var actualResult = actual as NoContentResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        _ordersServiceMock.Verify(c => c.UpdateOrderStatus(order.Id, status), Times.Once);
    }

    [Test]
    public async Task UpdateOrderPaymentStatus_WhenValidRequestPassed_NoContentReceived()
    {
        //given
        var order = new Order()
        {
            Id = 1,
        };
        var status = PaymentStatus.Paid;

        //when
        var actual = await _sut.UpdateOrderPaymentStatus(order.Id, status);

        //then
        var actualResult = actual as NoContentResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));
        _ordersServiceMock.Verify(c => c.UpdateOrderPaymentStatus(order.Id, status), Times.Once);
    }

    [Test]
    public async Task CreateOrder_WhenValidRequestPassed_CreatedResultReceived()
    {
        //given
        int expectedId = 1;
        var order = new OrderRequest()
        {
            StartTime = DateTime.UtcNow,
            BundlesIds = new() { 1, 2, 3},
            CleaningObjectId = 1,
            ServicesIds = new() { 1, 2, 3 },
        };

        var orderBusinessModel = _mapper.Map<OrderBusinessModel>(order);
        _ordersServiceMock.Setup(c => c.AddOrder(It.IsAny<OrderBusinessModel>(), It.IsAny<UserValues>())).ReturnsAsync(expectedId);

        //when
        var actual = await _sut.AddOrder(order);

        //then
        var actualResult = actual.Result as CreatedResult;

        Assert.That(actualResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That((int)actualResult.Value, Is.EqualTo(expectedId));
        _ordersServiceMock.Verify(x => x.AddOrder(It.Is<OrderBusinessModel>(c => 
        c.StartTime == order.StartTime &&
        c.Bundles.Count == order.BundlesIds.Count && 
        c.CleaningObject.Id == order.CleaningObjectId && 
        c.Services.Count == order.ServicesIds.Count), It.IsAny<UserValues>()), Times.Once);
    }
}
