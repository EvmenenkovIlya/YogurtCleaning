using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class CleanersService : ICleanersService
{
    private readonly ICleanersRepository _cleanersRepository;
    private readonly IOrdersRepository _ordersRepository;

    public CleanersService(ICleanersRepository cleanersRepository, IOrdersRepository ordersRepository)
    {
        _cleanersRepository = cleanersRepository;
        _ordersRepository = ordersRepository;
    }

    public void UpdateCleaner(Cleaner modelToUpdate, int id)
    {
        Cleaner cleaner = _cleanersRepository.GetCleaner(id);

        cleaner.FirstName = modelToUpdate.FirstName;
        cleaner.LastName = modelToUpdate.LastName;
        cleaner.Services = modelToUpdate.Services;
        cleaner.BirthDate = modelToUpdate.BirthDate;
        cleaner.Phone = modelToUpdate.Phone;
        _cleanersRepository.UpdateCleaner(cleaner);
    }

    // получаем список тех, кто работает в этот день. проверяем их заказы на этот день и пересечения по времени + 1 час на дорогу
    // проверяем список сервисов?? район?
    public List<Cleaner> GetFreeCleanersForOrder(Order order)
    {
        var freeCleaners = new List<Cleaner>();
        var workingCleaners = _cleanersRepository.GetAllCleaners()
            .Where(c => (c.Schedule is Schedule.ShiftWork && Convert.ToInt32((order.StartTime - c.DateOfStartWork).TotalDays % 4) < 2) ||
            (c.Schedule is Schedule.FullTime && order.StartTime.DayOfWeek != DayOfWeek.Sunday && order.StartTime.DayOfWeek != DayOfWeek.Saturday))
            .ToList();

        foreach (var c in workingCleaners)
        {
            bool isMatch = true;

            foreach (var o in c.Orders)
            {
                if (o.StartTime.Date != order.StartTime.Date || ((o.StartTime >= order.EndTime.AddHours(1) || o.EndTime.AddHours(1) <= order.StartTime)))
                {
                    isMatch = true;
                }
                else
                {
                    isMatch = false;
                    break;
                }
            }

            if (isMatch is true)
            {
                freeCleaners.Add(c);
            }
        }
        return freeCleaners;
    }
}