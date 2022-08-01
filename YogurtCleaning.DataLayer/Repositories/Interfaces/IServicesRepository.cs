using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public interface IServicesRepository
{
    Service GetService(int id);
    List<Service> GetAllServices();
    void UpdateService(Service service);
    int AddService(Service service);
    void DeleteService(Service service);
}