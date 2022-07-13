using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories.Intarfaces
{
    public interface ICleaningObjectsRepository
    {
        int CreateCleaningObject(CleaningObject cleaningObject);
        void DeleteCleaningObject(int cleaningObjectId);
        List<CleaningObject> GetAllCleaningObjects();
    }
}