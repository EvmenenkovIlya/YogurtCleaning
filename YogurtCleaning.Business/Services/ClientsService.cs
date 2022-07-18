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

        if (client == null || client.Id == 0)
        {
            throw new EntityNotFoundException($"Client {id} not found");
        }
        if (userValues.Email == (string)client.Email || userValues.Role == Role.Admin.ToString())
            return client;
        else
        {
            throw new AccessException($"Access denied");
        }
    }

    public List<Client> GetAllClients(UserValues userValues)
    {
        if (userValues.Role == Role.Admin.ToString())
        {
            var clients = _clientsRepository.GetAllClients();
            return clients;
        }
        else
        {
            throw new AccessException($"Access denied");
        }
    }

    public void DeleteClient(int id, UserValues userValues)
    {
        var client = _clientsRepository.GetClient(id);
        if (client == null || client.Id == 0)
        {
            throw new EntityNotFoundException($"Client {id} not found");
        }
        if (!(userValues.Email == client.Email || userValues.Role == Role.Admin.ToString()))
        {
            throw new AccessException($"Access denied");
        }
        else
            _clientsRepository.DeleteClient(id);
    }

    public void UpdateClient(Client modelToUpdate, int id, UserValues userValues)
    {
        Client client = _clientsRepository.GetClient(id);
        if (client == null || client.Id == 0)
        {
            throw new EntityNotFoundException($"Client {id} not found");
        }
        if (!(userValues.Email == (string)client.Email))
        {
            throw new AccessException($"Access denied");
        }
        else
        {
            client.FirstName = modelToUpdate.FirstName;
            client.LastName = modelToUpdate.LastName;
            client.Phone = modelToUpdate.Phone;
            client.BirthDate = modelToUpdate.BirthDate;
            _clientsRepository.UpdateClient(client);
        }
    }

    public int CreateClient(Client client)
    {
        // add checking password and confirm password
        var isChecked = CheckEmailForUniqueness(client.Email);

        if (!isChecked)
        {
            throw new UniquenessException($"That email is registred");
        }
        else
            return _clientsRepository.CreateClient(client);

    }
    public List<Comment> GetCommentsByClient(int id, UserValues userValues)
    {
        var client = _clientsRepository.GetClient(id);
        var comments = _clientsRepository.GetAllCommentsByClient(id);

        if (client == null || client.Id == 0)
        {
            throw new EntityNotFoundException($"Client {id} not found");
        }

        if (!(userValues.Email == (string)client.Email || userValues.Role == Role.Admin.ToString()))
        {
            throw new AccessException($"Access denied");
        }
        return comments;
    }

    public List<Order> GetOrdersByClient(int id, UserValues userValues)
    {
        var client = _clientsRepository.GetClient(id);
        var orders = _clientsRepository.GetAllOrdersByClient(id);


        if (client == null || client.Id == 0)
        {
            throw new EntityNotFoundException($"Orders by client {id} not found");
        }
        if (!(userValues.Email == (string)client.Email || userValues.Role == Role.Admin.ToString()))
        {
            throw new AccessException($"Access denied");
        }
        else
            return orders;
    }

    private bool CheckEmailForUniqueness(string email) => _clientsRepository.GetClientByEmail(email) == null;
}