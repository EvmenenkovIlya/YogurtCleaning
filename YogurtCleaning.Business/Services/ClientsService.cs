using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class ClientsService : IClientsService
{
    private readonly IClientsRepository _clientsRepository;

    public ClientsService(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }

    public void UpdateClient(Client newClient, int id)
    {
        Client oldClient = _clientsRepository.GetClient(id);

        oldClient.FirstName = newClient.FirstName;
        oldClient.LastName = newClient.LastName;
        oldClient.Phone = newClient.Phone;
        oldClient.BirthDate = newClient.BirthDate;
        _clientsRepository.UpdateClient(oldClient);
    }
}