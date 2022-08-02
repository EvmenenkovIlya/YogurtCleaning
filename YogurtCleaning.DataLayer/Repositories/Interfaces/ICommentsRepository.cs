using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public interface ICommentsRepository
{
    List<Comment> GetAllComments();
    int AddComment(Comment comment);
    void DeleteComment(Comment comment);
    Comment? GetCommentById(int id);
}
