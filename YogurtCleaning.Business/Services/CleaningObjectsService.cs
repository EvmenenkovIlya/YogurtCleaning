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

    public async Task<int> CreateCleaningObject(CleaningObject cleaningObject, UserValues userValues)
    {
        if (userValues.Role == Role.Admin)
        {
            cleaningObject.Client = await _clientsRepository.GetClient(cleaningObject.Client.Id);
        }
        else
        {
            cleaningObject.Client = await _clientsRepository.GetClient(userValues.Id);
        }
        cleaningObject.District = await _cleaningObjectsRepository.GetDistrict(cleaningObject.District.Id);
        Validator.CheckThatObjectNotNull(cleaningObject.Client, ExceptionsErrorMessages.ClientNotFound);
        Validator.CheckThatObjectNotNull(cleaningObject.District, ExceptionsErrorMessages.DistrictNotFound);
        return await _cleaningObjectsRepository.CreateCleaningObject(cleaningObject);
    }

    public async Task<CleaningObject> GetCleaningObject(int cleaningObjectId, UserValues userValues)
    {
        var cleaningObject = await _cleaningObjectsRepository.GetCleaningObject(cleaningObjectId); ;
        if (cleaningObject == null)
        {
            throw new EntityNotFoundException($"Cleaning Object with Id {cleaningObjectId} not found");
        }
        AuthorizeEnitiyAccess(cleaningObject, userValues);
        return cleaningObject;
    }

    public async Task<List<CleaningObject>> GetAllCleaningObjectsByClientId(int clientId, UserValues userValues)
    {
        var client = await _clientsRepository.GetClient(clientId);
        Validator.CheckThatObjectNotNull(client, ExceptionsErrorMessages.ClientNotFound);
        if (!(clientId == userValues.Id || userValues.Role == Role.Admin))
        {
            throw new AccessException($"Access denied");
        }
        return await _cleaningObjectsRepository.GetAllCleaningObjectsByClientId(clientId);
    } 

    public async Task UpdateCleaningObject(CleaningObject modelToUpdate, int id, UserValues userValues)
    {
        var cleaningObject = await _cleaningObjectsRepository.GetCleaningObject(id);
        Validator.CheckThatObjectNotNull(cleaningObject, ExceptionsErrorMessages.CleaningObjectNotFound);
        Validator.AuthorizeEnitiyAccess(cleaningObject, userValues);

        cleaningObject.NumberOfRooms = modelToUpdate.NumberOfRooms;
        cleaningObject.NumberOfBathrooms = modelToUpdate.NumberOfBathrooms;
        cleaningObject.NumberOfWindows = modelToUpdate.NumberOfWindows;
        cleaningObject.NumberOfBalconies = modelToUpdate.NumberOfBalconies;
        cleaningObject.Address = modelToUpdate.Address;
        cleaningObject.Square = modelToUpdate.Square;
        cleaningObject.District = await _cleaningObjectsRepository.GetDistrict(modelToUpdate.District.Id);
        Validator.CheckThatObjectNotNull(cleaningObject.District, ExceptionsErrorMessages.DistrictNotFound);
        await _cleaningObjectsRepository.UpdateCleaningObject(cleaningObject);
    }

    public async Task DeleteCleaningObject(int id, UserValues userValues)
    {
        var cleaningObject = await _cleaningObjectsRepository.GetCleaningObject(id);
        Validator.CheckThatObjectNotNull(cleaningObject, ExceptionsErrorMessages.CleaningObjectNotFound);
        Validator.AuthorizeEnitiyAccess(cleaningObject, userValues);
        await _cleaningObjectsRepository.DeleteCleaningObject(cleaningObject);
    }

    private void AuthorizeEnitiyAccess(CleaningObject cleaningObject, UserValues userValues)
    {
        if (!(userValues.Id == cleaningObject.Client.Id || userValues.Role == Role.Admin))
        {
            throw new AccessException($"Access denied");
        }
    }
}
