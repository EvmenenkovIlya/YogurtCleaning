using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;

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

    public List<Cleaner> GetWorkingCleanersForDate(DateTime orderDate)
    {
        var workingCleaners = GetAllCleaners()
            .Where(c => (c.Schedule is Schedule.ShiftWork && Convert.ToInt32((orderDate - c.DateOfStartWork).TotalDays % 4) < 2) ||
            (c.Schedule is Schedule.FullTime && orderDate.DayOfWeek != DayOfWeek.Sunday && orderDate.DayOfWeek != DayOfWeek.Saturday))
            .ToList();
        return workingCleaners;
    }
}
