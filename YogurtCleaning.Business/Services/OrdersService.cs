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
    private readonly ICleaningObjectsRepository _cleaningObjectsRepository;
    private readonly ICleanersRepository _cleanersRepository;
    private readonly IEmailSender _emailSender;
    private readonly IMapper _mapper;

    public OrdersService(IOrdersRepository ordersRepository, 
        ICleanersService cleanersService, 
        IClientsRepository clientsRepository, 
        IBundlesRepository bundlesRepository,
        ICleaningObjectsRepository cleaningObjectsRepository,
        ICleanersRepository cleanersRepository,
        IEmailSender emailSender, 
        IMapper mapper)
    {
        _ordersRepository = ordersRepository;
        _cleanersService = cleanersService;
        _clientsRepository = clientsRepository;
        _bundlesRepository = bundlesRepository;
        _cleaningObjectsRepository = cleaningObjectsRepository;
        _cleanersRepository = cleanersRepository;
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
        modelToUpdate.Client = order.Client;
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

    
    public async Task<int> AddOrder(OrderBusinessModel order, UserValues userValues)
    {
        Validator.AuthorizeEnitiyAccess(userValues, order.Client.Id);
        await FillOrder(order);
        var orderToSave = _mapper.Map<Order>(order);
        GetPropertiesFromDb(ref orderToSave, order);
        orderToSave.Bundles = await _bundlesRepository.GetBundles(orderToSave.Bundles);
        var result = await _ordersRepository.CreateOrder(orderToSave);
        CheckStatusAndSendMail(order, result);
        return result;
    }

    private void GetPropertiesFromDb(ref Order orderToSave, OrderBusinessModel order)
    {
        orderToSave.Services = order.Services;
        orderToSave.CleaningObject = order.CleaningObject;
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
        order.Services = await _cleanersRepository.GetServices(order.Services);
        order.Client = await _clientsRepository.GetClient(order.Client.Id);
        order.CleaningObject = await _cleaningObjectsRepository.GetCleaningObject(order.CleaningObject.Id);
        Validator.CheckThatObjectNotNull(order.Client, ExceptionsErrorMessages.ClientNotFound);
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
            if (lastOrder != null && (lastOrder.Bundles[0].Type == CleaningType.General || lastOrder.Bundles[0].Type == CleaningType.AfterRenovation))
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
