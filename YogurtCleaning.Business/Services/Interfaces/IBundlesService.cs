using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services;

public interface IBundlesService
{
    Task UpdateBundle(Bundle bundle, int id);
    Task<Bundle> GetBundle(int id);
    Task<List<Bundle>> GetAllBundles();
    Task<int> AddBundle(Bundle bundle);
    Task DeleteBundle(int id);
    Task<List<Service>> GetAdditionalServices(int id);
}