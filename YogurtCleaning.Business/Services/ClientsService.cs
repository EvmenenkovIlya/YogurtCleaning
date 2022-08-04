using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Business.Exceptions;

namespace YogurtCleaning.Business.Services;

public class ClientsService : IClientsService
{
    private readonly IClientsRepository _clientsRepository;

    public ClientsService(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }

    public async Task<Client?> GetClient(int id, UserValues userValues)
    {
        var client = await _clientsRepository.GetClient(id);

        if (client == null)
        {
            throw new EntityNotFoundException($"Client {id} not found");
        }
        AuthorizeEnitiyAccess(userValues, client);
        return client;
    }

    public async Task<List<Client>> GetAllClients()
    {
            var clients = await _clientsRepository.GetAllClients();
            return clients;
    }

    public async Task DeleteClient(int id, UserValues userValues)
    {
        var client = await _clientsRepository.GetClient(id);
        Validator.CheckThatObjectNotNull(client, ExceptionsErrorMessages.ClientNotFound);
        AuthorizeEnitiyAccess(userValues, client!);
        await _clientsRepository.DeleteClient(client!);
    }

    public async Task UpdateClient(Client modelToUpdate, int id, UserValues userValues)
    {
        var client = await _clientsRepository.GetClient(id);
        Validator.CheckThatObjectNotNull(client, ExceptionsErrorMessages.ClientNotFound);
        AuthorizeEnitiyAccess(userValues, client!);

        client!.FirstName = modelToUpdate.FirstName;
        client.LastName = modelToUpdate.LastName;
        client.Phone = modelToUpdate.Phone;
        client.BirthDate = modelToUpdate.BirthDate;
        await _clientsRepository.UpdateClient(client);
    }

    public async Task<int> CreateClient(Client client)
    {
        var isChecked = await CheckEmailForUniqueness(client.Email);

        if (!isChecked)
        {
            throw new UniquenessException($"That email is registred");
        }
        else
        client.Password = PasswordHash.HashPassword(client.Password);
        return await _clientsRepository.CreateClient(client);

    }

    public async Task<List<Comment>> GetCommentsByClient(int id, UserValues userValues)
    {
        var client = await _clientsRepository.GetClient(id);

        Validator.CheckThatObjectNotNull(client, ExceptionsErrorMessages.ClientCommentsNotFound);
        AuthorizeEnitiyAccess(userValues, client!);
        return await _clientsRepository.GetAllCommentsByClient(id);
    }

    public async Task<List<Order>> GetOrdersByClient(int id, UserValues userValues)
    {
        var client = await _clientsRepository.GetClient(id);
        Validator.CheckThatObjectNotNull(client, ExceptionsErrorMessages.ClientOrdersNotFound);
        AuthorizeEnitiyAccess(userValues, client!);        
        return await _clientsRepository.GetAllOrdersByClient(id);
            
    }

    private async Task<bool> CheckEmailForUniqueness(string email) => await _clientsRepository.GetClientByEmail(email) == null;

    private void AuthorizeEnitiyAccess(UserValues userValues, Client client)
    {
        if (!(userValues.Email == client.Email || userValues.Role == Role.Admin))
        {
            throw new AccessException($"Access denied");
        }
    }  
}