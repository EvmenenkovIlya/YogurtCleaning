using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services
{
    public interface ICleaningObjectsService
    {
        int CreateCleaningObject(CleaningObject cleaningObject, UserValues userValues);
        void UpdateCleaningObject(CleaningObject modelToUpdate, int id, UserValues userValues);
    }
}