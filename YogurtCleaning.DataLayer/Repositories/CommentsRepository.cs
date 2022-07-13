using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public class CommentsRepository : ICommentsRepository
{
    private readonly YogurtCleaningContext _context;

    public CommentsRepository(YogurtCleaningContext context)
    {
        _context = context;
    }

    public int AddComment(Comment comment)
    {
        _context.Comments.Add(comment);
        _context.SaveChanges();

        return comment.Id;
    }

    public void DeleteComment(int id)
    {
        var comment = _context.Comments.FirstOrDefault(c => c.Id == id);
        comment.IsDeleted = true;
        _context.SaveChanges();
    }

    public List<Comment> GetAllComments() => _context.Comments.Where(c => !c.IsDeleted).ToList();
}
