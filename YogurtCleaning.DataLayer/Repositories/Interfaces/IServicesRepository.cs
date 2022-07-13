using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories.Interfaces;

public interface IServicesRepository
{
    int GetService(int id);
    List<Service> GetAllServices();
    void UpdateService(Service service);
    int AddService(Service service);
    void DeleteService(int id);
}
