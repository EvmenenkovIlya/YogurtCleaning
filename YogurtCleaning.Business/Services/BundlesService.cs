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

    public async Task<int> AddBundle(Bundle bundle)
    {
        List<Service> services = bundle.Services;
        bundle.Services = await _bundlesRepository.GetServices(bundle.Services);
        Validator.CheckRequestAndDbList(services, bundle.Services);
        var result = await _bundlesRepository.AddBundle(bundle);
        return result;
    }

    public async Task DeleteBundle(int id)
    {
        var bundle = await _bundlesRepository.GetBundle(id);
        Validator.CheckThatObjectNotNull(bundle, ExceptionsErrorMessages.BundleNotFound);
        await _bundlesRepository.DeleteBundle(bundle);
    }

    public async Task<List<Bundle>> GetAllBundles()
    {
        var result = await _bundlesRepository.GetAllBundles();
        return result;
    }

    public async Task<Bundle> GetBundle(int id)
    {
        var result = await _bundlesRepository.GetBundle(id);
        Validator.CheckThatObjectNotNull(result, ExceptionsErrorMessages.BundleNotFound);
        return result;
    }

    public async Task UpdateBundle(Bundle bundle, int id)
    {
        var oldBundle = await _bundlesRepository.GetBundle(id);
        oldBundle.Name = bundle.Name;
        oldBundle.Measure = bundle.Measure;
        oldBundle.Price = bundle.Price;
        oldBundle.Services = bundle.Services;

        await _bundlesRepository.UpdateBundle(oldBundle);
    }

    public async Task<List<Service>> GetAdditionalServices(int id)
    {
        var bundle = await _bundlesRepository.GetBundle(id);
        var allServices = await _servicesRepository.GetAllServices();
        var bundleServiceIds = bundle.Services.Select(t => t.Id).ToList();
        var result = allServices.Where(t => !bundleServiceIds.Contains(t.Id)).ToList();
        return result;
    }
}