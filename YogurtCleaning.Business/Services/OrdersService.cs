using YogurtCleaning.Business.Exceptions;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
﻿using AutoMapper;
using YogurtCleaning.Business.Models;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class OrdersService : IOrdersService
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly ICleanersService _cleanersService;
    private readonly IClientsRepository _clientsRepository;
    private readonly IBundlesRepository _bundlesRepository;
    private readonly IEmailSender _emailSender;
    private readonly IMapper _mapper;

    public OrdersService(IOrdersRepository ordersRepository, 
        ICleanersService cleanersService, 
        IClientsRepository clientsRepository, 
        IBundlesRepository bundlesRepository,
        IEmailSender emailSender, 
        IMapper mapper)
    {
        _ordersRepository = ordersRepository;
        _cleanersService = cleanersService;
        _clientsRepository = clientsRepository;
        _bundlesRepository = bundlesRepository;
        _emailSender = emailSender;
        _mapper = mapper;
    }

    public async Task DeleteOrder(int id, UserValues userValues)
    {
        var order = await _ordersRepository.GetOrder(id);
        Validator.CheckThatObjectNotNull(order, ExceptionsErrorMessages.OrderNotFound);
        Validator.AuthorizeEnitiyAccess(userValues, order.Client.Id);
        await _ordersRepository.DeleteOrder(order);
    }

    public async Task<Order?> GetOrder(int id, UserValues userValues)
    {
        var order = await _ordersRepository.GetOrder(id);
        Validator.CheckThatObjectNotNull(order, ExceptionsErrorMessages.OrderNotFound);
        Validator.AuthorizeEnitiyAccess(order!, userValues);
        return order;
    }

    public async Task <List<Order>> GetAllOrders() => await _ordersRepository.GetAllOrders();

    public async Task UpdateOrder(OrderBusinessModel modelToUpdate, int id, UserValues userValues)
    {
        Order order = await _ordersRepository.GetOrder(id);
        Validator.CheckThatObjectNotNull(order, ExceptionsErrorMessages.OrderNotFound);
        Validator.AuthorizeEnitiyAccess(userValues, order.Client.Id);
        order!.Status = modelToUpdate.Status;
        order.StartTime = modelToUpdate.StartTime;
        order.UpdateTime = modelToUpdate.UpdateTime;
        modelToUpdate.CleaningObject = order.CleaningObject;
        await FillOrder(modelToUpdate);
        order.Bundles = _mapper.Map<List<Bundle>>(modelToUpdate.Bundles);
        order.Services = modelToUpdate.Services;
        order.CleanersBand = modelToUpdate.CleanersBand;

        await _ordersRepository.UpdateOrder(order);
    }

    public async Task<CleaningObject> GetCleaningObject(int orderId, UserValues userValues)
    {
        var order = await GetOrder(orderId, userValues);
        Validator.CheckThatObjectNotNull(order, ExceptionsErrorMessages.OrderNotFound);
        Validator.AuthorizeEnitiyAccess(order, userValues);
        return order.CleaningObject;
    }

    
    public async Task<int> AddOrder(OrderBusinessModel order)
    {
        await FillOrder(order);
        var result = await _ordersRepository.CreateOrder(_mapper.Map<Order>(order));
        CheckStatusAndSendMail(order, result);
        return result;
    }

    private void CheckStatusAndSendMail(OrderBusinessModel order, int orderId)
    {
        if (order.Status == Status.Moderation)
        {
            _emailSender.SendEmail(orderId);
        }
    }

    private async Task FillOrder(OrderBusinessModel order)
    {
        order.Bundles = await GetBundles(order);
        order.Price = await GetOrderPrice(order);
        order.CleanersBand = await GetCleanersForOrder(order);
        if (order.CleanersBand.Count < order.CleanersCount)
        {
            order.Status = Status.Moderation;
        }
        else
        {
            order.Status = Status.Created;
        }
    }

    private async Task<decimal> GetOrderPrice(OrderBusinessModel order)
    {

        order.Bundles.ForEach(b => b.SetPriceForCleaningObject(order.CleaningObject));

        var bundlesPrice = order.Bundles.Select(b => b.PriceForCleaningObject).Sum();
        var servicesPrice = order.Services.Select(s => s.Price).Sum();
        var orderPrice = bundlesPrice + servicesPrice;

        if (order.Bundles[0].Type == CleaningType.Regular)
        {
            var discount = (decimal)0.2;
            var lastOrder = await _clientsRepository.GetLastOrderForCleaningObject(order.Client.Id, order.CleaningObject.Id);
            if (lastOrder != null && lastOrder.Bundles[0].Type == CleaningType.General || lastOrder.Bundles[0].Type == CleaningType.AfterRenovation)
            {
                orderPrice -= orderPrice * discount;
            }
        }
        return orderPrice;
    }

    private async Task<List<Cleaner>> GetCleanersForOrder(OrderBusinessModel order)
    {
        var cleaners = new List<Cleaner>();
        var freeCleaners = await _cleanersService.GetFreeCleanersForOrder(order);

        if (order.EndTime > order.StartTime.Date.AddHours(18))
        {
            freeCleaners = freeCleaners.FindAll(c => c.Schedule == Schedule.ShiftWork).ToList();
        }

        if (freeCleaners.Count < order.CleanersCount)
        {
            cleaners.AddRange(freeCleaners);
        }
        else
        {
            Random random = new Random(); 
            freeCleaners = freeCleaners.OrderBy(x => random.Next()).ToList();
            
            for (int i = 0; i < order.CleanersCount; i++)
            {
                cleaners.Add(freeCleaners[i]);
            }
        }
        return cleaners;
    }

    private async Task<List<BundleBusinessModel>> GetBundles(OrderBusinessModel order)
    {
        var fullBundles = new List<BundleBusinessModel>();
        foreach(var b in order.Bundles)
        {
            var fullBundle = _mapper.Map<BundleBusinessModel>(await _bundlesRepository.GetBundle(b.Id));
            fullBundles.Add(fullBundle);
        }
        return fullBundles;
    }
}
