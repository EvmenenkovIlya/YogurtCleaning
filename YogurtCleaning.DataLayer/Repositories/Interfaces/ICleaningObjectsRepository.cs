using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.DataLayer.Repositories;

public interface ICleaningObjectsRepository
{
    Task<int> CreateCleaningObject(CleaningObject cleaningObject);
    Task DeleteCleaningObject(CleaningObject cleaningObject);
    Task<List<CleaningObject>> GetAllCleaningObjectsByClientId(int clientId);
    Task<CleaningObject?> GetCleaningObject(int cleaningObjectId);
    Task UpdateCleaningObject(CleaningObject newProperty);
    Task<District> GetDistrict(DistrictEnum district);
}