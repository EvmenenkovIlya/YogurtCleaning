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

    public async Task<Client?> GetClient(int clientId) => 
        await _context.Clients.FirstOrDefaultAsync(o => o.Id == clientId);

    public async Task<List<Client>> GetAllClients() => 
        await _context.Clients.Where(o => !o.IsDeleted).AsNoTracking().ToListAsync();

    public async Task<int> CreateClient(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client.Id;
    }

    public async Task UpdateClient(Client newClient)
    {
        _context.Clients.Update(newClient);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteClient(Client client)
    {
        client.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Comment>> GetAllCommentsByClient(int clientId) => 
        await _context.Comments.Where(c => c.Client != null && c.Client.Id == clientId).ToListAsync();

    public async Task<List<Order>> GetAllOrdersByClient(int id) => await _context.Orders.Where(o => o.Client.Id == id).ToListAsync();

    public async Task<Client?> GetClientByEmail(string email) => await _context.Clients.FirstOrDefaultAsync(o => o.Email == email);

    public async Task UpdateClientRating(int id)
    {
        var orders = await GetAllOrdersByClient(id);
        var comments = new List<Comment>();
        foreach (var order in orders)
        {
            comments.Add(await _context.Comments.FirstOrDefaultAsync(c => c.Order.Id == order.Id && c.Cleaner != null));
        }
        var clientRating = (decimal)(comments.Select(c => c.Rating).Sum()) / (decimal)comments.Count();

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        client.Rating = clientRating;
        await UpdateClient(client);
        await _context.SaveChangesAsync();
    }
}
