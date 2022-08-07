﻿using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public class CleanersRepository : ICleanersRepository
{
    private readonly YogurtCleaningContext _context;

    public CleanersRepository(YogurtCleaningContext context)
    {
        _context = context;
    }

    public async Task<Cleaner?> GetCleaner(int clientId) => await _context.Cleaners.FirstOrDefaultAsync(o => o.Id == clientId);

    public async Task<List<Cleaner>> GetAllCleaners()
    {
        return await _context.Cleaners.AsNoTracking().Where(o => !o.IsDeleted).ToListAsync();
    }

    public async Task<int> CreateCleaner(Cleaner cleaner)
    {
        _context.Cleaners.Add(cleaner);
        await _context.SaveChangesAsync();
        return cleaner.Id;
    }

    public async Task UpdateCleaner(Cleaner modelToUdate)
    {
        _context.Update(modelToUdate);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCleaner(Cleaner cleaner)
    {
        cleaner.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Comment>> GetAllCommentsByCleaner(int clientId) =>
        await _context.Comments.Where(c => c.Cleaner != null && c.Cleaner.Id == clientId).ToListAsync();

    public async Task<List<Order>> GetAllOrdersByCleaner(int id) => await _context.Orders.Where(o => o.CleanersBand.Any(c => c.Id == id)).ToListAsync();

    public async Task<Cleaner?> GetCleanerByEmail(string email) => await _context.Cleaners.FirstOrDefaultAsync(o => o.Email == email);

    public async Task UpdateCleanerRating(int id)
    {
        var orders = await GetAllOrdersByCleaner(id);
        var comments = new List<Comment>();
        foreach (var order in orders)
        {
            comments.Add(await _context.Comments.FirstOrDefaultAsync(c => c.Order.Id == order.Id && c.Client != null));
        }
        var cleanerRating = (decimal)(comments.Select(c => c.Rating).Sum())/(decimal)comments.Count();

        var cleaner = await _context.Cleaners.FirstOrDefaultAsync(c => c.Id == id);
        cleaner.Rating = cleanerRating;
        await UpdateCleaner(cleaner);
        await _context.SaveChangesAsync();
    }
}
