using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public interface ICommentsRepository
{
    List<Comment> GetAllComments();
    int AddComment(Comment comment);
    void DeleteComment(int id);
    Comment? GetCommentById(int id);
}
