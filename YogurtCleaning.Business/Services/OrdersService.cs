﻿using AutoMapper;
using YogurtCleaning.Business.Models;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class OrdersService : IOrdersService
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly ICleanersService _cleanersService;
    private readonly IClientsRepository _clientsRepository;
    private readonly IEmailSender _emailSender;
    private readonly IMapper _mapper;

    public OrdersService(IOrdersRepository ordersRepository, ICleanersService cleanersService, IClientsRepository clientsRepository, IEmailSender emailSender, IMapper mapper)
    {
        _ordersRepository = ordersRepository;
        _cleanersService = cleanersService;
        _clientsRepository = clientsRepository;
        _emailSender = emailSender;
        _mapper = mapper;
    }

    public void UpdateOrder(Order modelToUpdate, int id)
    {
        Order order = _ordersRepository.GetOrder(id);

        order.Status = modelToUpdate.Status;
        order.StartTime = modelToUpdate.StartTime;
        order.UpdateTime = modelToUpdate.UpdateTime;
        order.Bundles = modelToUpdate.Bundles;
        order.Services = modelToUpdate.Services;
        order.CleanersBand = modelToUpdate.CleanersBand;
        _ordersRepository.UpdateOrder(order);
    }
    
    public int AddOrder(OrderBusinessModel order)
    {
        order.Price = GetOrderPrice(order);
        order.TotalDuration = order.GetTotalDuration();
        order.CleanersCount = order.GetCleanersCount();
        order.EndTime = order.GetEndTime();
        order.CleanersBand = GetCleanersForOrder(order); 
        if(order.CleanersBand.Count < order.CleanersCount)
        {
            order.Status = Status.Moderation;
        }
        else
        {
            order.Status = Status.Created;
        }

        var result = _ordersRepository.CreateOrder(_mapper.Map<Order>(order));

        if (order.Status == Status.Moderation)
        {
            _emailSender.SendEmail(result);
        }
        return result;
    } 

    private decimal GetOrderPrice(OrderBusinessModel order)
    {
        var bundlesPrice = order.Bundles.Select(b => b.GetPrice(order.CleaningObject)).Sum();
        var servicesPrice = order.Services.Select(s => s.Price).Sum();
        var orderPrice = bundlesPrice + servicesPrice;
        if (order.Bundles[0].Type == CleaningType.Regular)
        {
            var discount = (decimal)0.2;
            var lastOrder = _clientsRepository.GetLastOrderForCleaningObject(order.Client.Id, order.CleaningObject.Id);
            if (lastOrder != null && lastOrder.Bundles[0].Type == CleaningType.General || lastOrder.Bundles[0].Type == CleaningType.AfterRenovation)
            {
                orderPrice -= orderPrice * discount;
            }
        }
        return orderPrice;
    }

    private List<Cleaner> GetCleanersForOrder(OrderBusinessModel order)
    {
        var cleaners = new List<Cleaner>();
        //var cleanersCount = GetCleanersCount(order);
        var freeCleaners = _cleanersService.GetFreeCleanersForOrder(order);

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
}
