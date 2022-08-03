using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.DataLayer.Repositories;

public interface IOrdersRepository
{
    int CreateOrder(Order order);
    void DeleteOrder(Order order);
    Order? GetOrder(int id);
    List<Order> GetAllOrders();
    void UpdateOrder(Order order);
    List<Service> GetServices(int orderId);
    void UpdateOrderStatus(int orderId, Status statusEnam);
    CleaningObject GetCleaningObject(int orderId);
}