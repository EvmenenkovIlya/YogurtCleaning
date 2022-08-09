﻿using Microsoft.EntityFrameworkCore;
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

    public async Task<Order?> GetLastOrderForCleaningObject(int clientId, int cleaningObjectId)
    {
        var clientOrders = (await GetAllOrdersByClient(clientId)).Where(o => o.CleaningObject.Id == cleaningObjectId);
        var lastOrder = clientOrders.FirstOrDefault(o => o.StartTime == ((clientOrders.Select(o => o.StartTime)).Max()));
        return lastOrder;
    }

    public async Task<List<Comment>> GetCommentsAboutClient(int id)
    {
        var orders = await GetAllOrdersByClient(id);
        var comments = new List<Comment>();
        foreach (var order in orders)
        {
            comments.AddRange(await _context.Comments.Where(c => c.Order.Id == order.Id && c.Cleaner != null).ToListAsync());
        }
        return comments;
    }
}
