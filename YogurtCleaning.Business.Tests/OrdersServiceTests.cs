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
    private Mock<IOrdersRepository> _mockOrdersRepository;
    private Mock<ICleanersService> _mockCleanersService;
    private Mock<IClientsRepository> _mockClientsRepository;
    private Mock<IEmailSender> _mockEmailSender;
    private OrdersService _sut;


    public OrdersServiceTests()
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

        var order = new Order
        {
            Id = 10,
            Client = new() { Id = 11},
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
            Status = Status.Edited,
            StartTime = DateTime.Now.AddDays(2),
            UpdateTime = DateTime.Now,
            Bundles = new List<Bundle> { new() { Id = 2, Name = "qwe" }, new() { Id = 22, Name = "qwa" } },
            Services = new List<Service> { new Service { Id = 3456} },
            CleanersBand = new List<Cleaner> { new() { Id = 654 }, new() { Id = 777} }
        };

        _mockOrdersRepository.Setup(o => o.GetOrder(order.Id)).Returns(order);

        // when
        _sut.UpdateOrder(updatedOrder, order.Id);

        // then
        _mockOrdersRepository.Verify(o => o.UpdateOrder(
            It.Is<Order>(
                i => i.Id == order.Id
                && i.Client == order.Client
                && i.CleaningObject == order.CleaningObject
                && i.Status == updatedOrder.Status
                && i.StartTime == updatedOrder.StartTime
                && i.UpdateTime == updatedOrder.UpdateTime
                && i.Bundles == updatedOrder.Bundles
                && i.Services == updatedOrder.Services
                && i.CleanersBand == updatedOrder.CleanersBand)),
            Times.Once);
        _mockOrdersRepository.Verify(o => o.GetOrder(order.Id), Times.Once);
    }
}