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

    public int AddCommentt(Comment comment)
    {
        throw new NotImplementedException();
    }

    public void DeleteCommentt(Comment comment)
    {
        throw new NotImplementedException();
    }

    public List<Comment>? GetAllComments()
    {
        throw new NotImplementedException();
    }
}
