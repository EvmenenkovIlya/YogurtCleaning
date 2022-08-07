using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.DataLayer.Repositories;

public interface IOrdersRepository
{
    int CreateOrder(Order order);
    void DeleteOrder(Order order);
    Task<Order?> GetOrder(int id);
    List<Order> GetAllOrders();
    void UpdateOrder(Order order);
    Task<List<Service>> GetServices(int orderId);
    Task UpdateOrderStatus(int orderId, Status statusEnam);
    Task<CleaningObject> GetCleaningObject(int orderId);
}