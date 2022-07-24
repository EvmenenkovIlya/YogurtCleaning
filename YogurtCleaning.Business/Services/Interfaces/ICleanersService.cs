using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services
{
    public interface ICleanersService
    {
        void UpdateCleaner(Cleaner newCleaner, int id, UserValues userValies);
        Cleaner? GetCleaner(int id, UserValues userValies);
        List<Cleaner> GetAllCleaners();
        void DeleteCleaner(int id, UserValues userValies);
        int CreateCleaner(Cleaner cleaner);
        List<Comment> GetCommentsByCleaner(int id, UserValues userValies);
        List<Order> GetOrdersByCleaner(int id, UserValues userValies);
        List<Cleaner> GetFreeCleanersForOrder(Order order);
    }
}