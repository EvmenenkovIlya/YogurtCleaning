using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services;

public interface IBundlesService
{
    void UpdateBundle(Bundle bundle, int id);
}