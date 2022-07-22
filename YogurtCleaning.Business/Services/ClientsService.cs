using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;
using System.Security.Claims;
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

    public Client? GetClient(int id, UserValues userValues)
    {
        var client = _clientsRepository.GetClient(id);

        CheckThatUserNotNull(client, ExceptionsErrorMessages.ClientNotFound);
        CheckPossibilityOfAccess(userValues, client);
        return client;
    }

    public List<Client> GetAllClients()
    {
            var clients = _clientsRepository.GetAllClients();
            return clients;
    }

    public void DeleteClient(int id, UserValues userValues)
    {
        var client = _clientsRepository.GetClient(id);
        CheckThatUserNotNull(client, ExceptionsErrorMessages.ClientNotFound);
        CheckPossibilityOfAccess(userValues, client);
        _clientsRepository.DeleteClient(id);
    }

    public void UpdateClient(Client modelToUpdate, int id, UserValues userValues)
    {
        Client client = _clientsRepository.GetClient(id);
        CheckThatUserNotNull(client, ExceptionsErrorMessages.ClientNotFound);
        CheckPossibilityOfAccess(userValues, client);

        client.FirstName = modelToUpdate.FirstName;
        client.LastName = modelToUpdate.LastName;
        client.Phone = modelToUpdate.Phone;
        client.BirthDate = modelToUpdate.BirthDate;
        _clientsRepository.UpdateClient(client);

    }

    public int CreateClient(Client client)
    {
        var isChecked = CheckEmailForUniqueness(client.Email);

        if (!isChecked)
        {
            throw new UniquenessException($"That email is registred");
        }
        else
        client.Password = PasswordHash.HashPassword(client.Password);
        return _clientsRepository.CreateClient(client);

    }
    public List<Comment> GetCommentsByClient(int id, UserValues userValues)
    {
        var client = _clientsRepository.GetClient(id);

        CheckThatUserNotNull(client, ExceptionsErrorMessages.ClientCommentsNotFound);
        CheckPossibilityOfAccess(userValues, client);
        return _clientsRepository.GetAllCommentsByClient(id);
    }

    public List<Order> GetOrdersByClient(int id, UserValues userValues)
    {
        var client = _clientsRepository.GetClient(id);
        CheckThatUserNotNull(client, ExceptionsErrorMessages.ClientOrdersNotFound);
        CheckPossibilityOfAccess(userValues, client);        
        return _clientsRepository.GetAllOrdersByClient(id);
            
    }

    private bool CheckEmailForUniqueness(string email) => _clientsRepository.GetClientByEmail(email) == null;

    private void CheckPossibilityOfAccess(UserValues userValues, Client client)
    {
        if (!(userValues.Email == client.Email || userValues.Role == Role.Admin.ToString()))
        {
            throw new AccessException($"Access denied");
        }
    }

    private void CheckThatUserNotNull(Client client, string errorMesage)
    {
        if (client == null)
        {
            throw new BadRequestException(errorMesage);
        }
    }
}