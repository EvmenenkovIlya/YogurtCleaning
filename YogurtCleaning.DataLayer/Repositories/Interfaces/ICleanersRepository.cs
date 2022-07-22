using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public interface ICleanersRepository
{
    int CreateCleaner(Cleaner cleaner);
    Cleaner? GetCleaner(int clientId);
    void UpdateCleaner(Cleaner cleaner);
    void DeleteCleaner(int cleanerId);
    List<Cleaner> GetAllCleaners();
    List<Comment> GetAllCommentsByCleaner(int cleanerId);
    List<Order> GetAllOrdersByCleaner(int id);
    Cleaner? GetCleanerByEmail(string email);
    Cleaner? GetCleanerByLogin(LoginData login);
}