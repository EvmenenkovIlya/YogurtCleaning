using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services;

public interface IBundlesService
{
    void UpdateBundle(Bundle bundle, int id);
    Bundle GetBundle(int id);
    List<Bundle> GetAllBundles();
    int AddBundle(Bundle bundle);
    void DeleteBundle(int id);
    List<Service> GetAdditionalServices(int id);
}