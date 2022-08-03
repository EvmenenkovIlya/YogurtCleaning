using YogurtCleaning.Business.Models;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services
{
    public interface ICleanersService
    {
        Task UpdateCleaner(Cleaner newCleaner, int id, UserValues userValies);
        Task<Cleaner?> GetCleaner(int id, UserValues userValies);
        Task<List<Cleaner>> GetAllCleaners();
        Task DeleteCleaner(int id, UserValues userValies);
        Task<int> CreateCleaner(Cleaner cleaner);
        Task<List<Comment>> GetCommentsByCleaner(int id, UserValues userValies);
        Task<List<Order>> GetOrdersByCleaner(int id, UserValues userValies);
        List<Cleaner> GetFreeCleanersForOrder(OrderBusinessModel order);
    }
}