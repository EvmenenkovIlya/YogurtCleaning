using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services
{
    public interface ICleaningObjectsService
    {
        void UpdateCleaningObject(CleaningObject modelToUpdate, int id);
        int CreateCleaningObject(CleaningObject cleaningObject, UserValues userValues);
    }
}