using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using YogurtCleaning.DataLayer.Entities;
using System.Threading.Tasks;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.Business.Exceptions;

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
    public int AddBundle(Bundle bundle)
    {
        var result = _bundlesRepository.AddBundle(bundle);
        return result;
    }

    public void DeleteBundle(int id)
    {
        var bundle = _bundlesRepository.GetBundle(id);
        Validator.CheckThatObjectNotNull(bundle, ExceptionsErrorMessages.BundleNotFound);
        _bundlesRepository.DeleteBundle(bundle);
    }

    public List<Bundle> GetAllBundles()
    {
        var result = _bundlesRepository.GetAllBundles();
        return result;
    }
    public Bundle GetBundle(int id)
    {
        var result = _bundlesRepository.GetBundle(id);
        return result;
    }

    public void UpdateBundle(Bundle bundle, int id)
    {
        var oldBundle = _bundlesRepository.GetBundle(id);
        oldBundle.Name = bundle.Name;
        oldBundle.Measure = bundle.Measure;
        oldBundle.Price = bundle.Price;
        oldBundle.Services = bundle.Services;

        _bundlesRepository.UpdateBundle(oldBundle);
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