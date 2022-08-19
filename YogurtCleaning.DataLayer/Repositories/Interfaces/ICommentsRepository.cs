using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public interface ICommentsRepository
{
    Task<List<Comment>> GetAllComments();
    Task<int> AddComment(Comment comment);
    Task DeleteComment(Comment comment);
    Task<Comment?> GetCommentById(int id);
}
