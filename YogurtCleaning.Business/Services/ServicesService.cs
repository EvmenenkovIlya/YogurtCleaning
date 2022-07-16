using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public void UpdateService(Service service, int id)
    {
        var oldService = _servicesRepository.GetService(id);
        oldService.Name = service.Name;
        oldService.Price = service.Price;
        oldService.Unit = service.Unit;
        oldService.Duration = service.Duration;
        _servicesRepository.UpdateService(oldService);
    }

    public List<Service> GetAdditionalServicesForBundle(Bundle bundle)
    {
        var result = new List<Service>();
        var allServices = _servicesRepository.GetAllServices();
        var bundleServices = bundle.Services;
        foreach(var s in allServices)
        {
            foreach(var bs in bundleServices)
            {
                if (s.Id != bs.Id)
                    result.Add(s);
            }
        }
        return result;
    }
}
