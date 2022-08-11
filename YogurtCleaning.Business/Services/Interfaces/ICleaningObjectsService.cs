using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services
{
    public interface ICleaningObjectsService
    {
        Task<int> CreateCleaningObject(CleaningObject cleaningObject, UserValues userValues);
        Task UpdateCleaningObject(CleaningObject modelToUpdate, int id, UserValues userValues);
        Task DeleteCleaningObject(int id, UserValues userValues);
        Task<CleaningObject> GetCleaningObject(int cleaningObjectId, UserValues userValues);
        Task<List<CleaningObject>> GetAllCleaningObjectsByClientId(int clientId, UserValues userValues);
    }
}