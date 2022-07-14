using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class CleaningObjectsService : ICleaningObjectsService
{
    private readonly ICleaningObjectsRepository _cleaningObjectsRepository;

    public CleaningObjectsService(ICleaningObjectsRepository cleaningObjectsRepository)
    {
        _cleaningObjectsRepository = cleaningObjectsRepository;
    }

    public void UpdateCleaningObject(CleaningObject modelToUpdate, int id)
    {
        var cleaningObject = _cleaningObjectsRepository.GetCleaningObject(id);
        cleaningObject.NumberOfRooms = modelToUpdate.NumberOfRooms;
        cleaningObject.NumberOfBathrooms = modelToUpdate.NumberOfBathrooms;
        cleaningObject.NumberOfWindows = modelToUpdate.NumberOfWindows;
        cleaningObject.NumberOfBalconies = modelToUpdate.NumberOfBalconies;
        cleaningObject.Address = modelToUpdate.Address;
        cleaningObject.Square = modelToUpdate.Square;
        _cleaningObjectsRepository.UpdateCleaningObject(cleaningObject);
    }
}
