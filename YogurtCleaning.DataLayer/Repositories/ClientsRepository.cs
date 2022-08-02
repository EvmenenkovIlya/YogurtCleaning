using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public class ClientsRepository : IClientsRepository
{
    private readonly YogurtCleaningContext _context;

    public ClientsRepository(YogurtCleaningContext context)
    {
        _context = context;
    }

    public Client? GetClient(int clientId) => 
        _context.Clients.FirstOrDefault(o => o.Id == clientId);

    public List<Client> GetAllClients() => 
        _context.Clients.Where(o => !o.IsDeleted).AsNoTracking().ToList();

    public int CreateClient(Client client)
    {
        _context.Clients.Add(client);
        _context.SaveChanges();
        return client.Id;
    }

    public void UpdateClient(Client newClient)
    {
        _context.Clients.Update(newClient);
        _context.SaveChanges();
    }

    public void DeleteClient(Client client)
    {
        client.IsDeleted = true;
        _context.SaveChanges();
    }

    public List<Comment> GetAllCommentsByClient(int clientId) => 
        _context.Comments.Where(c => c.Client != null && c.Client.Id == clientId).ToList();

    public List<Order> GetAllOrdersByClient(int id) => _context.Orders.Where(o => o.Client.Id == id).ToList();

    public Client? GetClientByEmail(string email) => _context.Clients.FirstOrDefault(o => o.Email == email);
}
