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
        throw new NotImplementedException();
    }

    public void DeleteBundle(int id)
    {
        throw new NotImplementedException();
    }

    public List<Bundle> GetAllBundles()
    {
        throw new NotImplementedException();
    }

    public Bundle GetBundle(int id)
    {
        throw new NotImplementedException();
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