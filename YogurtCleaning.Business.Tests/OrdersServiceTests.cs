using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.Business.Models;
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
    private IMapper _mapper;
    private OrdersService _sut;


    public OrdersServiceTests()
    {
        _mockClientsRepository = new Mock<IClientsRepository>();
        _mockCleanersService = new Mock<ICleanersService>();
        _mockOrdersRepository = new Mock<IOrdersRepository>();
        _mockEmailSender = new Mock<IEmailSender>();
        var mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new BusinessMapperConfigStorage());
        });
        _mapper = mapper.CreateMapper();
        _sut = new OrdersService(_mockOrdersRepository.Object, _mockCleanersService.Object, _mockClientsRepository.Object, _mockEmailSender.Object, _mapper);
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

    [Fact]
    public void AddOrderTest_WhenCleanersIsEnough_ThenOrderStatusIsCreated()
    {
        // given
        var cleaners = new List<Cleaner>
        {
            new Cleaner()
            {
                Id = 11,
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com1",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
                Schedule = Schedule.FullTime,
                Orders = new List<Order>(),
                DateOfStartWork = new DateTime(2022, 8, 1, 00, 00, 00)
            },
            new Cleaner()
            {
                Id = 13,
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com2",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
                Schedule = Schedule.ShiftWork,
                Orders = new List<Order>(),
                DateOfStartWork = new DateTime(2022, 8, 1, 00, 00, 00)
            }
        };
        var order = new OrderBusinessModel
        {
            Client = new() { Id = 11 },
            CleaningObject = new() { Id = 56 },
            StartTime = new DateTime(2022, 8, 1, 14, 00, 00),
            Bundles = new List<BundleBusinessModel> { new BundleBusinessModel { Id = 2, Duration = 6, Measure = Measure.Apartment, Price = 100 } },
            Services = new List<Service> { new Service { Id = 42, Duration = 2, Price = 10 } }
        };

        decimal expectedPrice = 110;
        var expectedCleanersCount = 2;
        var expectedEndTime = new DateTime(2022, 8, 1, 18, 00, 00);
        var expectedStatus = Status.Created;

        _mockCleanersService.Setup(c => c.GetFreeCleanersForOrder(order)).Returns(cleaners);

        // when
        _sut.AddOrder(order);

        // then
        _mockOrdersRepository.Verify(o => o.CreateOrder(
            It.Is<Order>(
                i => i.Price == expectedPrice
                && i.CleanersBand.Count == expectedCleanersCount
                && i.Status == expectedStatus
                && i.EndTime == expectedEndTime)),
            Times.Once);
        _mockCleanersService.Verify(c => c.GetFreeCleanersForOrder(order), Times.Once);
    }

    [Fact]
    public void AddOrderTest_WhenCleanersIsNotEnough_ThenOrderStatusIsModeration()
    {
        // given
        var cleaners = new List<Cleaner>
        {
            new Cleaner()
            {
                Id = 11,
                FirstName = "Adam",
                LastName = "Smith",
                Password = "12345678",
                Email = "AdamSmith@gmail.com1",
                Phone = "85559997264",
                BirthDate = DateTime.Today,
                Schedule = Schedule.FullTime,
                Orders = new List<Order>(),
                DateOfStartWork = new DateTime(2022, 8, 1, 00, 00, 00)
            }
        };
        var order = new OrderBusinessModel
        {
            Client = new() { Id = 11 },
            CleaningObject = new() { Id = 56 },
            StartTime = new DateTime(2022, 8, 1, 14, 00, 00),
            Bundles = new List<BundleBusinessModel> { new BundleBusinessModel { Id = 2, Duration = 6, Measure = Measure.Apartment, Price = 100 } },
            Services = new List<Service> { new Service { Id = 42, Duration = 2, Price = 10 } }
        };

        decimal expectedPrice = 110;
        var expectedCleanersCount = 1;
        var expectedEndTime = new DateTime(2022, 8, 1, 18, 00, 00);
        var expectedStatus = Status.Moderation;

        _mockCleanersService.Setup(c => c.GetFreeCleanersForOrder(order)).Returns(cleaners);

        // when
        _sut.AddOrder(order);

        // then
        _mockOrdersRepository.Verify(o => o.CreateOrder(
            It.Is<Order>(
                i => i.Price == expectedPrice
                && i.CleanersBand.Count == expectedCleanersCount
                && i.Status == expectedStatus
                && i.EndTime == expectedEndTime)),
            Times.Once);
        _mockCleanersService.Verify(c => c.GetFreeCleanersForOrder(order), Times.Once);
        _mockEmailSender.Verify(e => e.SendEmail(It.IsAny<int>()), Times.Once);
    }
}