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

    public void UpdateClient(Client modelToUpdate, int id)
    {
        Client client = _clientsRepository.GetClient(id);

        client.FirstName = modelToUpdate.FirstName;
        client.LastName = modelToUpdate.LastName;
        client.Phone = modelToUpdate.Phone;
        client.BirthDate = modelToUpdate.BirthDate;
        _clientsRepository.UpdateClient(client);
    }
}