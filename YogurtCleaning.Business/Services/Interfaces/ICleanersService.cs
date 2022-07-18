using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services
{
    public interface ICleanersService
    {
        void UpdateCleaner(Cleaner modelToUpdate, int id);
    }
}