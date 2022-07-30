using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public interface ICleaningObjectsRepository
{
    int CreateCleaningObject(CleaningObject cleaningObject);
    void DeleteCleaningObject(CleaningObject cleaningObject);
    List<CleaningObject> GetAllCleaningObjects();
    CleaningObject? GetCleaningObject(int cleaningObjectId);
    void UpdateCleaningObject(CleaningObject newProperty);
}