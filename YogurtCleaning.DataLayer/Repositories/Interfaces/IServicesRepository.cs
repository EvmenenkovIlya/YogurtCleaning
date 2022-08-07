using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public interface IServicesRepository
{
    Task<Service> GetService(int id);
    Task<List<Service>> GetAllServices();
    Task UpdateService(Service service);
    Task<int> AddService(Service service);
    Task DeleteService(Service service);
}