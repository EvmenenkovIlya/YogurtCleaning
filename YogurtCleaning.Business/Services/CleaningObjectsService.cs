using YogurtCleaning.Business.Exceptions;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class CleaningObjectsService : ICleaningObjectsService
{
    private readonly ICleaningObjectsRepository _cleaningObjectsRepository;

    public CleaningObjectsService(ICleaningObjectsRepository cleaningObjectsRepository)
    {
        _cleaningObjectsRepository = cleaningObjectsRepository;
    }

    public int CreateCleaningObject(CleaningObject cleaningObject, UserValues userValues)
    {
        cleaningObject.Client = new Client() { Id = userValues.Id };
        return _cleaningObjectsRepository.CreateCleaningObject(cleaningObject);
    }

    public void UpdateCleaningObject(CleaningObject modelToUpdate, int id, UserValues userValues)
    {
        var cleaningObject = _cleaningObjectsRepository.GetCleaningObject(id);
        if (cleaningObject == null)
        {
            throw new BadRequestException($"Cleaninig object {id} not found");
        }
        AuthorizeEnitiyAccess(cleaningObject, userValues);

        cleaningObject.NumberOfRooms = modelToUpdate.NumberOfRooms;
        cleaningObject.NumberOfBathrooms = modelToUpdate.NumberOfBathrooms;
        cleaningObject.NumberOfWindows = modelToUpdate.NumberOfWindows;
        cleaningObject.NumberOfBalconies = modelToUpdate.NumberOfBalconies;
        cleaningObject.Address = modelToUpdate.Address;
        cleaningObject.Square = modelToUpdate.Square;

        _cleaningObjectsRepository.UpdateCleaningObject(cleaningObject);
    }

    public void DeleteCleaningObject(int id, UserValues userValues)
    {
        var cleaningObject = _cleaningObjectsRepository.GetCleaningObject(id);
        CheckThatCleaningObjectNotNull(cleaningObject, ExceptionsErrorMessages.CleaningObjectNotFound);
        AuthorizeEnitiyAccess(cleaningObject, userValues);
        _cleaningObjectsRepository.DeleteCleaningObject(id);
    }

    private void AuthorizeEnitiyAccess(CleaningObject cleaningObject, UserValues userValues)
    {
        if (!(userValues.Id == cleaningObject.Client.Id || userValues.Role == Role.Admin.ToString()))
        {
            throw new AccessException($"Access denied");
        }
    }

    private void CheckThatCleaningObjectNotNull(CleaningObject cleaningObject, string errorMesage)
    {
        if (cleaningObject == null)
        {
            throw new BadRequestException(errorMesage);
        }
    }
}
