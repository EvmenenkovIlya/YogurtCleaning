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

    public BundlesService(IBundlesRepository bundlesRepository)
    {
        _bundlesRepository = bundlesRepository;
    }

    public int AddBundle(Bundle bundle)
    {
        var result = _bundlesRepository.AddBundle(bundle);
        return result;
    }

    public void DeleteBundle(int id)
    {
        _bundlesRepository.DeleteBundle(id);
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
}