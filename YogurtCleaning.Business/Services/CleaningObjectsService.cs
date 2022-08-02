using YogurtCleaning.Business.Exceptions;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class CleaningObjectsService : ICleaningObjectsService
{
    private readonly ICleaningObjectsRepository _cleaningObjectsRepository;
    private readonly IClientsRepository _clientsRepository;

    public CleaningObjectsService(ICleaningObjectsRepository cleaningObjectsRepository, IClientsRepository clientsRepository)
    {
        _cleaningObjectsRepository = cleaningObjectsRepository;
        _clientsRepository = clientsRepository;
    }

    public int CreateCleaningObject(CleaningObject cleaningObject, UserValues userValues)
    {
        if (userValues.Role == Role.Admin)
        {
            cleaningObject.Client = _clientsRepository.GetClient(cleaningObject.Client.Id);
        }
        else
        {
            cleaningObject.Client = _clientsRepository.GetClient(userValues.Id);
        }
        cleaningObject.District = _cleaningObjectsRepository.GetDistrict(cleaningObject.District.Id);
        Validator.CheckThatObjectNotNull(cleaningObject.Client, ExceptionsErrorMessages.ClientNotFound);
        Validator.CheckThatObjectNotNull(cleaningObject.District, ExceptionsErrorMessages.DistrictNotFound);
        return _cleaningObjectsRepository.CreateCleaningObject(cleaningObject);
    }

    public CleaningObject GetCleaningObject(int cleaningObjectId, UserValues userValues)
    {
        var cleaningObject = _cleaningObjectsRepository.GetCleaningObject(cleaningObjectId); ;
        if (cleaningObject == null)
        {
            throw new EntityNotFoundException(ExceptionsErrorMessages.CleaningObjectNotFound);
        }
        if (!(userValues.Id == cleaningObject.Client.Id || !(userValues.Role == Role.Client)))
        {
            throw new AccessException($"Access denied");
        }
        return cleaningObject;

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
        Validator.CheckThatObjectNotNull(cleaningObject, ExceptionsErrorMessages.CleaningObjectNotFound);
        AuthorizeEnitiyAccess(cleaningObject, userValues);
        _cleaningObjectsRepository.DeleteCleaningObject(cleaningObject);
    }

    private void AuthorizeEnitiyAccess(CleaningObject cleaningObject, UserValues userValues)
    {
        if (!(userValues.Id == cleaningObject.Client.Id || userValues.Role == Role.Admin))
        {
            throw new AccessException($"Access denied");
        }
    }
}
