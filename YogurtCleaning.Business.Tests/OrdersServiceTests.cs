using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Tests;

public class OrdersServiceTests
{
    private OrdersService _sut;
    private Mock<IOrdersRepository> _mockOrdersRepository;
    private Mock<ICleanersService> _mockCleanersService;
    private Mock<IClientsRepository> _mockClientsRepository;
    private Mock<IEmailSender> _mockEmailSender;

    private void Setup()
    {
        _mockClientsRepository = new Mock<IClientsRepository>();
        _mockCleanersService = new Mock<ICleanersService>();
        _mockOrdersRepository = new Mock<IOrdersRepository>();
        _mockEmailSender = new Mock<IEmailSender>();
        _sut = new OrdersService(_mockOrdersRepository.Object, _mockCleanersService.Object, _mockClientsRepository.Object, _mockEmailSender.Object);
    }

    [Fact]
    public void UpdateOrder_WhenUpdatePassed_ThenPropertiesValuesChandged()
    {
        // given
        Setup();

        var order = new Order
        {
            Id = 10,
            Client = new() { Id = 1},
            CleaningObject = new() { Id = 56},
            Status = Status.Created,
            StartTime = DateTime.Now.AddDays(1),
            Bundles = new List<Bundle> { new Bundle { Id = 2, Name = "qwe" } },
            Services = null,
            CleanersBand = new List<Cleaner> {new() { Id = 654 } },
            IsDeleted = false
        };

        var updatedOrder = new Order
        {
            Client = new() { Id = 1 },
            CleaningObject = new() { Id = 56 },
            Status = Status.Edited,
            StartTime = DateTime.Now.AddDays(1),
            UpdateTime = DateTime.Now,
            Bundles = new List<Bundle> { new() { Id = 2, Name = "qwe" }, new() { Id = 22, Name = "qwa" } },
            Services = new List<Service> { new Service { Id = 3456} },
            CleanersBand = new List<Cleaner> { new() { Id = 654 }, new() { Id = 777} },
            IsDeleted = false
        };

        _mockOrdersRepository.Setup(o => o.GetOrder(order.Id)).Returns(order);

        // when
        _sut.UpdateOrder(updatedOrder, order.Id);

        // then
        Assert.True(order.Status == updatedOrder.Status);
        Assert.True(order.StartTime == updatedOrder.StartTime);
        Assert.True(order.UpdateTime == updatedOrder.UpdateTime);
        Assert.True(order.Bundles == updatedOrder.Bundles);
        Assert.True(order.Services == updatedOrder.Services);
        Assert.True(order.CleanersBand == updatedOrder.CleanersBand);
        _mockOrdersRepository.Verify(o => o.GetOrder(order.Id), Times.Once);
    }

    //[Fact]
    //public void AddOrder_WhenValidRequestPassed_OrderAdded()
    //{
    //    // given
    //    Setup();

    //    var expectedId = 1;

    //    _mockOrdersRepository.Setup(o => o.CreateOrder(It.IsAny<Order>())).Returns(1);

    //    var order = new Order
    //    {
    //        Client = new() { Id = 1 },
    //        CleaningObject = new() { Id = 56 },
    //        Status = Status.Created,
    //        StartTime = DateTime.Now.AddDays(1),
    //        Bundles = new List<Bundle> { new Bundle { Id = 2, Name = "qwe" } },
    //        Services = null,
    //        CleanersBand = new List<Cleaner> { new() { Id = 654 } }
    //    };

    //    // when
    //    var actual = _sut.AddOrder(order);

    //    // then
    //    Assert.True(actual == expectedId);
    //    _mockOrdersRepository.Verify(o => o.CreateOrder(It.IsAny<Order>()), Times.Once);

    //}
}
