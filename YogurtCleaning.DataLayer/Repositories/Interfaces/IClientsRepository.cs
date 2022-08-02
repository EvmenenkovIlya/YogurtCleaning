using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public interface IClientsRepository
{
    int CreateClient(Client client);
    void DeleteClient(Client client);
    List<Client> GetAllClients();
    List<Comment> GetAllCommentsByClient(int clientId);
    List<Order> GetAllOrdersByClient(int clientId);
    Client? GetClient(int clientId);
    void UpdateClient(Client client);
    Client? GetClientByEmail(string email);
}