using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services;

public interface IServicesService
{
    void UpdateService(Service service, int id);
    void DeleteService(int id);
}