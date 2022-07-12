using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories.Intarfaces
{
    public interface IClientsRepository
    {
        int CreateClient(Client client);
        void DeleteClient(int clientId);
        List<Client> GetAllClients();
        List<Comment> GetAllCommentsByClient(int clientId);
        Client? GetClient(int clientId);
        void UpdateClient(Client client);
    }
}