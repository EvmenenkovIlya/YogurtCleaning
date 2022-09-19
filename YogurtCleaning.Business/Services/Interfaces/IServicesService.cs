using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services;

public interface IServicesService
{
    Task UpdateService(Service service, int id);
    Task<Service> GetService(int id);
    Task<int> AddService(Service service);
    Task DeleteService(int id, UserValues userValues);
    Task<List<Service>> GetAllServices();
}