using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services
{
    public interface IClientsService
    {
        void UpdateClient(Client newClient, int id, UserValues userValues);
        Client? GetClient(int id, UserValues userValues);
        List<Client> GetAllClients(UserValues userValues);
        void DeleteClient(int id, UserValues userValues);
        int CreateClient(Client client);
        List<Comment> GetCommentsByClient(int id, UserValues userValues);
        List<Order> GetOrdersByClient(int id, UserValues userValues);
    }
}