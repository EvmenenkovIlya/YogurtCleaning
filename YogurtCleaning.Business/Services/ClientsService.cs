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

    public Client? GetClient(int id, List<string>? identities)
    {
        var client = _clientsRepository.GetClient(id);

        if (client == null || client.Id == 0)
        {
            throw new EntityNotFoundException($"Client {id} not found");
        }
        if (identities[0] == (string)client.Email || identities[1] == Role.Admin.ToString())
            return client;
        else
        {
            throw new AccessException($"Access denied");
        }
    }

    public List<Client> GetAllClients(List<string>? identities)
    {
        if (identities[1] == Role.Admin.ToString())
        {
            var clients = _clientsRepository.GetAllClients();
            return clients;
        }
        else
        {
            throw new AccessException($"Access denied");
        }
    }

    public void DeleteClient(int id, List<string> identities)
    {
        var client = _clientsRepository.GetClient(id);
        if (client == null || client.Id == 0)
        {
            throw new EntityNotFoundException($"Client {id} not found");
        }
        if (!(identities[0] == (string)client.Email || identities[1] == Role.Admin.ToString()))
        {

            throw new AccessException($"Access denied");

        }
        else
            _clientsRepository.DeleteClient(id);
    }

    public void UpdateClient(Client modelToUpdate, int id, List<string> identities)
    {
        Client client = _clientsRepository.GetClient(id);
        if (client == null || client.Id == 0)
        {
            throw new EntityNotFoundException($"Client {id} not found");
        }
        if (!(identities[0] == (string)client.Email || identities[1] == Role.Admin.ToString()))
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
        var isChecked = CheckingEmailForUniqueness(client.Email);

        if (isChecked)
        {
            throw new UniquenessException($"That email is registred");
        }
        if (client.Phone is not null)
        {
            if (!(client.Phone.StartsWith("+7") || client.Phone.StartsWith("8") && client.Phone.Length <= 11))
            {
                throw new DataException($"Invalid phone number");
            }
        }
        if (client.BirthDate > DateTime.Now)
        {
            throw new DataException($"Invalid birthday");
        }
        else
            return _clientsRepository.CreateClient(client);

    }
    public List<Comment> GetCommentsByClient(int id, List<string> identities)
    {
        var client = _clientsRepository.GetClient(id);
        var comments = _clientsRepository.GetAllCommentsByClient(id);

        if (client == null || client.Id == 0)
        {
            throw new EntityNotFoundException($"Client {id} not found");
        }

        if (!(identities[0] == (string)client.Email || identities[1] == Role.Admin.ToString()))
        {
            throw new AccessException($"Access denied");
        }
        return comments;
    }

    public List<Order> GetOrdersByClient(int id, List<string> identities)
    {
        var client = _clientsRepository.GetClient(id);
        var orders = _clientsRepository.GetAllOrdersByClient(id);


        if (client == null || client.Id == 0)
        {
            throw new EntityNotFoundException($"Orders by client {id} not found");
        }
        if (!(identities[0] == (string)client.Email || identities[1] == Role.Admin.ToString()))
        {
            throw new AccessException($"Access denied");
        }
        else
            return orders;
    }

    private bool CheckingEmailForUniqueness(string email)
    {
        var clients = _clientsRepository.GetAllClients();

        if (clients is not null)
        {
            var uniqueEmail = clients.Any(c => c.Email == email);
            return uniqueEmail;
        }
        else return false;
    }
}