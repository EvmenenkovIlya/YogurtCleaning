﻿using AutoMapper;
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

        Assert.AreEqual(StatusCodes.Status200OK, actualResult.StatusCode);
        Assert.True(ordersResponse.Count == orders.Count);
        Assert.True(ordersResponse[0].Client.Id == orders[0].Client.Id);
        Assert.True(ordersResponse[0].CleaningObject.Id == orders[0].CleaningObject.Id);
        Assert.True(ordersResponse[1].Price == orders[1].Price);
        Assert.True(ordersResponse[1].CleanersBand.Count == orders[1].CleanersBand.Count);
        Assert.True(ordersResponse[0].Comments.Count == orders[0].Comments.Count);
        Assert.True(ordersResponse[0].Bundles.Count == orders[0].Bundles.Count);
        Assert.True(ordersResponse[0].Services.Count == orders[0].Services.Count);
        Assert.True(ordersResponse[0].Price == orders[0].Price);
        Assert.True(ordersResponse[0].StartTime == orders[0].StartTime);
        Assert.True(ordersResponse[0].EndTime == orders[0].EndTime);   
        Assert.True(ordersResponse[0].UpdateTime == orders[0].UpdateTime);
        Assert.True(ordersResponse[0].Status == orders[0].Status);
        _ordersServiceMock.Verify(x => x.GetAllOrders(), Times.Once);
    }
}
