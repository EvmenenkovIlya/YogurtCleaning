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
        AuthorizeEnitiyAccess(userValues, order);
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

    public async Task UpdateOrder(Order modelToUpdate, int id)
    {
        Order order = await _ordersRepository.GetOrder(id);
        Validator.CheckThatObjectNotNull(order, ExceptionsErrorMessages.OrderNotFound);
        order!.Status = modelToUpdate.Status;
        order.StartTime = modelToUpdate.StartTime;
        order.UpdateTime = modelToUpdate.UpdateTime;
        order.Bundles = modelToUpdate.Bundles;
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

    private void AuthorizeEnitiyAccess(UserValues userValues, Order order)
    {
        if (!(userValues.Id == order.Client.Id || userValues.Role == Role.Admin))
        {
            throw new AccessException($"Access denied");
        }
    }
    
    public async Task<int> AddOrder(OrderBusinessModel order)
    {
        var fullBundles = new List<BundleBusinessModel>();
        foreach(var b in order.Bundles)
        {
            var fullBundle = _mapper.Map<BundleBusinessModel>(await _bundlesRepository.GetBundle(b.Id));
            fullBundles.Add(fullBundle);
        }
        order.Bundles = fullBundles;
        order.Price = await GetOrderPrice(order);
        order.CleanersBand = await GetCleanersForOrder(order); 
        if(order.CleanersBand.Count < order.CleanersCount)
        {
            order.Status = Status.Moderation;
        }
        else
        {
            order.Status = Status.Created;
        }

        var result = await _ordersRepository.CreateOrder(_mapper.Map<Order>(order));

        if (order.Status == Status.Moderation)
        {
            _emailSender.SendEmail(result);
        }
        return result;
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
                int cleanersWithSameDistrictCount = freeCleaners.Count(c => c.Districts.Contains(order.CleaningObject.District));
                if (freeCleaners.Count(c => c.Districts.Contains(order.CleaningObject.District)) != 0)
                {
                    freeCleaners = freeCleaners.OrderBy(x => random.Next()).ToList();
                    var cleaner = freeCleaners.First(c => c.Districts.Contains(order.CleaningObject.District));
                    cleaners.Add(cleaner);
                    freeCleaners.Remove(cleaner);
                }
                else
                {
                    cleaners.Add(freeCleaners[i - cleanersWithSameDistrictCount - 1]);
                }
            }
        }
        return cleaners;
    }

    public async Task UpdateOrderStatus(int orderId, Status status)
    {
        Order order = await _ordersRepository.GetOrder(orderId);
        Validator.CheckThatObjectNotNull(order, ExceptionsErrorMessages.OrderNotFound);
        order!.Status = status;
        await _ordersRepository.UpdateOrder(order);
    }

    public async Task UpdateOrderPaymentStatus(int orderId, PaymentStatus paymentStatus)
    {
        Order order = await _ordersRepository.GetOrder(orderId);
        Validator.CheckThatObjectNotNull(order, ExceptionsErrorMessages.OrderNotFound);
        order!.PaymentStatus = paymentStatus;
        await _ordersRepository.UpdateOrder(order);
    }

    public async Task<List<Service>> GetOrderServices(int orderId, UserValues userValues)
    {
        Order order = await _ordersRepository.GetOrder(orderId);
        Validator.CheckThatObjectNotNull(order, ExceptionsErrorMessages.OrderNotFound);
        return order.Services;
    }
}
