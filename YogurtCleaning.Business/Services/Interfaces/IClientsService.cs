using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services
{
    public interface IClientsService
    {
        Task UpdateClient(Client newClient, int id, UserValues userValues);
        Task<Client?> GetClient(int id, UserValues userValues);
        Task<List<Client>> GetAllClients();
        Task DeleteClient(int id, UserValues userValues);
        Task<int> CreateClient(Client client);
        Task<List<Comment>> GetCommentsByClient(int id, UserValues userValues);
        Task<List<Order>> GetOrdersByClient(int id, UserValues userValues);
    }
}