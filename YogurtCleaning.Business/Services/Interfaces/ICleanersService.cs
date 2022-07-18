using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services
{
    public interface ICleanersService
    {
        void UpdateCleaner(Cleaner newCleaner, int id, List<string> identities);
        Cleaner? GetCleaner(int id, List<string> identities);
        List<Cleaner> GetAllCleaners(List<string> identities);
        void DeleteCleaner(int id, List<string> identities);
        int CreateCleaner(Cleaner cleaner);
        List<Comment> GetCommentsByCleaner(int id, List<string> identities);
        List<Order> GetOrdersByCleaner(int id, List<string> identities);
    }
}