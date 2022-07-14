using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public class CleanersRepository : ICleanersRepository
{
    private readonly YogurtCleaningContext _context;

    public CleanersRepository(YogurtCleaningContext context)
    {
        _context = context;
    }

    public Cleaner? GetCleaner(int clientId) => _context.Cleaners.FirstOrDefault(o => o.Id == clientId);

    public List<Cleaner> GetAllCleaners()
    {
        return _context.Cleaners.AsNoTracking().Where(o => !o.IsDeleted).ToList();
    }

    public int CreateCleaner(Cleaner cleaner)
    {
        _context.Cleaners.Add(cleaner);
        _context.SaveChanges();
        return cleaner.Id;
    }

    public void UpdateCleaner(Cleaner modelToUdate)
    {
        

        _context.SaveChanges();
    }

    public void DeleteCleaner(int cleanerId)
    {
        var cleaner = GetCleaner(cleanerId);
        cleaner.IsDeleted = true;
        _context.SaveChanges();
    }

    public List<Comment> GetAllCommentsByCleaner(int cleanerId)
    {
        var comments = _context.Comments.Where(c => c.Cleaner != null && c.Cleaner.Id == cleanerId).ToList();
        return comments;
    }

    public List<Cleaner> GetAllCleanersByListIds(List<int> cleanersIds)
    {
        List<Cleaner> result = new List<Cleaner>();
        foreach (var cleanerId in cleanersIds)
        {
            Cleaner cleaner = GetCleaner(cleanerId);
            if (cleaner != null)
            {
                result.Add(cleaner);
            }
        }
        return result;
    }
}
