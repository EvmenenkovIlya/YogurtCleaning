using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public interface IBundlesRepository
{
    Task<Bundle> GetBundle(int id);
    Task<List<Bundle>> GetAllBundles();
    Task UpdateBundle(Bundle bundle);
    Task<int> AddBundle(Bundle bundle);
    Task DeleteBundle(Bundle bundle);
    Task<List<Service>> GetServices(List<Service> servicesIds);
    Task<List<Bundle>> GetBundles(List<Bundle> bundles);
}