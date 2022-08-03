using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public interface IClientsRepository
{
    Task<int> CreateClient(Client client);
    Task DeleteClient(Client client);
    Task<List<Client>> GetAllClients();
    Task<List<Comment>> GetAllCommentsByClient(int clientId);
    Task<List<Order>> GetAllOrdersByClient(int clientId);
    Task<Client?> GetClient(int clientId);
    Task UpdateClient(Client client);
    Task<Client?> GetClientByEmail(string email);
    Order? GetLastOrderForCleaningObject(int clientId, int cleaningObjectId);
}