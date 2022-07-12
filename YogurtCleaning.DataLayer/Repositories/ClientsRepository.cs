using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories.Intarfaces;

namespace YogurtCleaning.DataLayer.Repositories;

public class ClientsRepository : IClientsRepository
{
    private readonly YogurtCleaningContext _context;

    public ClientsRepository(YogurtCleaningContext context)
    {
        _context = context;
    }

    public Client? GetClient(int clientId) => _context.Clients
            .FirstOrDefault(o => o.Id == clientId && !o.IsDeleted);

    public List<Client> GetAllClients()
    {
        return _context.Clients.AsNoTracking().Where(o => !o.IsDeleted).ToList<Client>();
    }

    public int CreateClient(Client client)
    {
        _context.Add(client);
        _context.SaveChanges();
        return client.Id;
    }

    public void UpdateClient(Client client)
    {
        _context.Clients.Update(client);
        _context.SaveChanges();
    }

    public void DeleteClient(int clientId)
    {
        var client = _context.Clients.FirstOrDefault(o => o.Id == clientId);
        client.IsDeleted = true;
        _context.Clients.Update(client);
        _context.SaveChanges();
    }

    public List<Comment> GetAllCommentsByClient(int clientId)
    {
        var comments = _context.Comments.AsNoTracking().Where(c => !c.IsDeleted && c.Client.Id == clientId).ToList();
        return comments;
    }
}
