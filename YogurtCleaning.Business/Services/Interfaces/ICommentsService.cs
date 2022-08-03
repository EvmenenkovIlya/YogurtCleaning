using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services;

public interface ICommentsService
{
    Task<int> AddCommentByClient(Comment comment, int clientId);
    Task<int> AddCommentByCleaner(Comment comment, int cleanerId);
    List<Comment> GetComments();
    void DeleteComment(int id);
}
