using System.Security.Claims;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services
{
    public interface IClientsService
    {
        void UpdateClient(Client newClient, int id);
        Client? GetClient(int id, List<string> identities);
        List<Client> GetAllClients(List<string> identities);
        void DeleteClient(int id, List<string> identities);
        int CreateClient(Client client);
        List<Comment> GetCommentsByClient(int id, List<string> identities);
        List<Order> GetOrdersByClient(int id, List<string> identities);
    }
}