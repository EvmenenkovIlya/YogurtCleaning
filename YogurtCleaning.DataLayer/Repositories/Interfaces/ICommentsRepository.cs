using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public interface ICommentsRepository
{
    List<Comment> GetAllComments();
    int AddComment(Comment comment);
    void DeleteComment(int id);
}
