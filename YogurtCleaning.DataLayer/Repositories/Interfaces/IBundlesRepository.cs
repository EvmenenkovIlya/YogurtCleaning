using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public interface IBundlesRepository
{
    Bundle GetBundle(int id);
    List<Bundle> GetAllBundles();
    void UpdateBundle(Bundle bundle);
    int AddBundle(Bundle bundle);
    void DeleteBundle(Bundle bundle);
}