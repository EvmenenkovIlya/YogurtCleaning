using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public class CommentsRepository : ICommentsRepository
{
    private readonly YogurtCleaningContext _context;

    public CommentsRepository(YogurtCleaningContext context)
    {
        _context = context;
    }

    public async Task<int> AddComment(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return comment.Id;
    }

    public async Task DeleteComment(Comment comment)
    {
        comment.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Comment>> GetAllComments() => await _context.Comments.Include(c => c.Cleaner).Include(c => c.Client).Include(c => c.Order).Where(c => !c.IsDeleted).ToListAsync();

    public async Task<Comment?> GetCommentById(int id) => await _context.Comments.Include(c => c.Cleaner).Include(c => c.Client).Include(c => c.Order).Include(c => c.Client).FirstOrDefaultAsync(c => c.Id == id);
}
