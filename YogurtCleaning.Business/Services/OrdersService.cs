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

    public OrdersService(IOrdersRepository ordersRepository, ICleanersService cleanersService, IClientsRepository clientsRepository, IEmailSender emailSender)
    {
        _ordersRepository = ordersRepository;
        _cleanersService = cleanersService;
        _clientsRepository = clientsRepository;
        _emailSender = emailSender;
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
    
    public int AddOrder(Order order)
    {
        order.Price = GetOrderPrice(order);
        order.EndTime = GetOrderEndTime(order);
        order.CleanersBand = GetCleanersForOrder(order); 
        if(order.CleanersBand.Count < GetCleanersCount(order))
        {
            order.Status = Status.Moderation;
        }
        else
        {
            order.Status = Status.Created;
        }

        var result = _ordersRepository.CreateOrder(order);

        if (order.Status == Status.Moderation)
        {
            var message = new Message(new string[] { "yogurtcleaning@gmail.com" }, "Order needs cleaners!", $"Order {result} doesn't have enought cleaners");
            _emailSender.SendEmail(message);
        }
        return result;
    }

    private decimal GetOrderPrice(Order order)
    {
        var bundlesPrice = order.Bundles.Select(b => GetBundlePricePerParameters(b, order.CleaningObject)).Sum();
        var servicesPrice = order.Services.Select(s => s.Price).Sum();
        var orderPrice = bundlesPrice + servicesPrice;
        if (order.Bundles[0].Type == CleaningType.Regular)
        {
            var clientOrders = _clientsRepository.GetAllOrdersByClient(order.Client.Id).Where(o => o.CleaningObject.Id == order.CleaningObject.Id);
            var lastOrder = clientOrders.FirstOrDefault(o => o.StartTime == ((clientOrders.Select(o => o.StartTime)).Max()));
            if (lastOrder != null || lastOrder.Bundles[0].Type == CleaningType.General || lastOrder.Bundles[0].Type == CleaningType.AfterRenovation)
            {
                orderPrice = orderPrice * (decimal)0.8;
            }
        }
        return orderPrice;
    }

    private decimal GetOrderDuration(Order order)
    {
        var bundlesDuration = order.Bundles.Select(b => GetBundleDurationPerParameters(b, order.CleaningObject)).Sum();
        var servicesDuration = order.Services.Select(s => s.Duration).Sum();
        var orderDuration = bundlesDuration + servicesDuration;
        return orderDuration;
    }

    private List<Cleaner> GetCleanersForOrder(Order order)
    {
        var cleaners = new List<Cleaner>();
        var cleanersCount = GetCleanersCount(order);
        var freeCleaners = _cleanersService.GetFreeCleanersForOrder(order);

        if (order.EndTime > order.StartTime.Date.AddHours(18))
        {
            freeCleaners = freeCleaners.FindAll(c => c.Schedule == Schedule.ShiftWork).ToList();
        }

        if (freeCleaners.Count < cleanersCount)
        {
            for (int i = 0; i < freeCleaners.Count; i++)
            {
                cleaners.Add(freeCleaners[i]);
            }
        }
        else
        {
            for (int i = 0; i < cleanersCount; i++)
            {
                cleaners.Add(freeCleaners[i]);
            }
        }
        return cleaners;
    }

    private int GetCleanersCount(Order order)
    {
        var orderDurationInHours = (double)GetOrderDuration(order);
        var maxOrderEndTime = order.StartTime.Date.AddHours(21);
        var maxOrderDuration = (maxOrderEndTime - order.StartTime).TotalHours;

        if (orderDurationInHours > maxOrderDuration)
        {
            var count = Convert.ToInt32(Math.Ceiling(orderDurationInHours / maxOrderDuration));
            return count;
        }
        else return 1;
    }

    private DateTime GetOrderEndTime(Order order)
    {
        var endTime = order.StartTime.AddHours((double)GetOrderDuration(order) / GetCleanersCount(order));
        return endTime;

    }

    private decimal GetBundlePricePerParameters(Bundle bundle, CleaningObject cleaningObject)
    {
        var price = bundle.Price;
        switch (bundle.Measure)
        {
            case Measure.Room:
                price += price / 2 * (cleaningObject.NumberOfRooms-1);
                return price;
            case Measure.Apartment:
                return price;
            case Measure.SquareMeter:
                price = price * cleaningObject.Square;
                return price;
            case Measure.Unit:
                price = price * cleaningObject.NumberOfWindows + price * cleaningObject.NumberOfBalconies;
                return price;
        }
        return price;
    }

    private decimal GetBundleDurationPerParameters(Bundle bundle, CleaningObject cleaningObject)
    {
        var duration = bundle.Duration;
        switch (bundle.Measure)
        {
            case Measure.Room:
                duration += duration / 2 * (cleaningObject.NumberOfRooms - 1);
                return duration;
            case Measure.Apartment:
                return duration;
            case Measure.SquareMeter:
                duration = duration * cleaningObject.Square;
                return duration;
            case Measure.Unit:
                duration = duration * cleaningObject.NumberOfWindows + duration * cleaningObject.NumberOfBalconies;
                return duration;
        }
        return duration;
    }
}
