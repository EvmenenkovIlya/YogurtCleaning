using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services;

public interface IServicesService
{
    void UpdateService(Service service, int id);
    Service GetService(int id);
    int AddService(Service service);
}