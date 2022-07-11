using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories.Intarfaces
{
    public interface IOrdersRepository
    {
        int CreateOrder(Order order);
        void DeleteOrder(int id);
        Order? GetOrderById(int id);
        List<Order> GetOrders();
        void UpdateOrder(Order order);
    }
}