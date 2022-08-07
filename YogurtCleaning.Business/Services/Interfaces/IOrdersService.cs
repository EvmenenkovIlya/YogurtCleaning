using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services
{
    public interface IOrdersService
    {
        Order? GetOrder(int id, UserValues userValues);
        Task UpdateOrder(Order modelToUpdate, int id);
        Task DeleteOrder(int id, UserValues userValues);
        List<Order> GetAllOrders();
    }
}