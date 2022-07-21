using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class CleanersService : ICleanersService
{
    private readonly ICleanersRepository _cleanersRepository;

    public CleanersService(ICleanersRepository cleanersRepository)
    {
        _cleanersRepository = cleanersRepository;
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

    public List<Cleaner> GetFreeCleanersForOrder(Order order)
    {
        return new List<Cleaner>(); // получаем список тех, кто работает в этот день. проверяем их заказы на этот день и пересечения по времени + 1 час на дорогу
                                    // проверяем список сервисов?? район?
    }
}