using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class BundlesService : IBundlesService
{
    private readonly IBundlesRepository _bundlesRepository;
    private readonly IServicesRepository _servicesRepository;

    public BundlesService(IBundlesRepository bundlesRepository, IServicesRepository servicesRepository)
    {
        _bundlesRepository = bundlesRepository;
        _servicesRepository = servicesRepository;
    }

    public List<Service> GetAdditionalServices(int id)
    {
        var bundle = _bundlesRepository.GetBundle(id);
        var allServices = _servicesRepository.GetAllServices();
        var bundleServiceIds = bundle.Services.Select(t => t.Id).ToList();
        var result = allServices.Where(t => !bundleServiceIds.Contains(t.Id)).ToList();
        return result;
    }
}
