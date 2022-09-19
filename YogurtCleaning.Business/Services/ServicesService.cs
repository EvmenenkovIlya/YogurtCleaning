using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class ServicesService : IServicesService
{
    private readonly IServicesRepository _servicesRepository;

    public ServicesService(IServicesRepository servicesRepository)
    {
        _servicesRepository = servicesRepository;
    }

    public async Task UpdateService(Service service, int id)
    {
        var oldService = await _servicesRepository.GetService(id);
        oldService.Name = service.Name;
        oldService.Price = service.Price;
        oldService.Unit = service.Unit;
        oldService.Duration = service.Duration;
        oldService.RoomType = service.RoomType;
        await _servicesRepository.UpdateService(oldService); 
    }
    public async Task<Service> GetService(int id) => await _servicesRepository.GetService(id);

    public async Task<int> AddService(Service service) => await _servicesRepository.AddService(service);
    public async Task DeleteService(int id, UserValues userValues)
    {
        var service = await _servicesRepository.GetService(id);
        Validator.CheckThatObjectNotNull(service, ExceptionsErrorMessages.ServiceNotFound);
       await _servicesRepository.DeleteService(service);
    }

    public async Task<List<Service>> GetAllServices() => await _servicesRepository.GetAllServices();
}
