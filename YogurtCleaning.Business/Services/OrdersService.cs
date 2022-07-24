using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class OrdersService : IOrdersService
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly ICleanersService _cleanersService;

    public OrdersService(IOrdersRepository ordersRepository, ICleanersService cleanersService)
    {
        _ordersRepository = ordersRepository;
        _cleanersService = cleanersService;
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

    //    Получаем реквест:
    //КлинингОбджект
    //Дата старта (дата старта д.б. в рабочие часы - валидация реквеста)
    //Лист бандлов
    //Лист допсервисов
    
    public int AddOrder(Order order)
    {
        order.Price = GetOrderPrice(order);
        order.EndTime = GetOrderEndTime(order);
        order.CleanersBand = GetCleanersForOrder(order);
        order.Status = Status.Created;
        var result = _ordersRepository.CreateOrder(order); // в какой момент добавляем в БД? до назначения клинеров?
        return result;
    }

    //Считаем цену:
    //цена бандла * параметры клининг обджекта + цены допсервисов
    private decimal GetOrderPrice(Order order)
    {
        var bundlesPrice = order.Bundles.Select(b => GetBundlePricePerParameters(b, order.CleaningObject)).Sum();
        var servicesPrice = order.Services.Select(s => s.Price).Sum();
        var orderPrice = bundlesPrice + servicesPrice;
        return orderPrice; // добавить скидку на поддерживающую после генеральной
    }

    //Считаем длительность заказа:
    //длительность бандла * параметры + длительность допсервисов
    private decimal GetOrderDuration(Order order)
    {
        var bundlesDuration = order.Bundles.Select(b => GetBundleDurationPerParameters(b, order.CleaningObject)).Sum();
        var servicesDuration = order.Services.Select(s => s.Duration).Sum();
        var orderDuration = bundlesDuration + servicesDuration;
        return orderDuration;
    }

    //Ищем людей:
    //в зависимости от времени окончания уборки(и следовательно длительности рабочей смены) - 2/2 или 5/2
    //Получаем список тех, кто работает в этот день и свободен в это время
    //Если нашли - назначаем
    //Если не нашли(или нашли меньше, чем нужно) -> менеджер
    private List<Cleaner> GetCleanersForOrder(Order order)
    {
        var cleaners = new List<Cleaner>();
        var cleanersCount = GetCleanersCount(order);
        var freeCleaners = _cleanersService.GetFreeCleanersForOrder(order);

        if (freeCleaners.Count < cleanersCount)
        {
            throw new Exception("Manager has to do some magic"); // как передаем запрос менеджеру?
        }
        else
        {
            if (order.EndTime <= order.StartTime.Date.AddHours(18))
            {
                for (int i = 0; i < cleanersCount; i++)
                {
                    cleaners.Add(freeCleaners[i]);
                    freeCleaners[i].Orders.Add(order);
                    _cleanersService.UpdateCleaner(freeCleaners[i], freeCleaners[i].Id);
                }
                return cleaners;
            }
            else
            {
                var freeShiftCleaners = freeCleaners.FindAll(c => c.Schedule == Schedule.ShiftWork).ToList();
                for (int i = 0; i < cleanersCount; i++)
                {
                    cleaners.Add(freeShiftCleaners[i]);
                }
                return cleaners;
            }
        }
    }

    //проверяем их заказы на этот день и пересечения по времени + 1 час на дорогу

    //Считаем количество клинеров:
    //чтобы уложиться до конца рабочего дня( 8 или 12-часового)
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

    //Считаем время выполнения заказа -> устанавливаем дату окончания
    private DateTime GetOrderEndTime(Order order)
    {
        var endTime = order.StartTime.AddHours((double)GetOrderDuration(order) / GetCleanersCount(order));
        return endTime;

    }

    // тут как-то надо сопоставить measure бандла с количеством чего-то всякого в ClObj
    // коэффициент?
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
                return duration;
        }
        return duration;
    }
}
