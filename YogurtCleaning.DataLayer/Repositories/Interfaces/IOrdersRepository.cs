using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.DataLayer.Repositories;

public interface IOrdersRepository
{
    Task <int> CreateOrder(Order order);
    Task DeleteOrder(Order order);
    Task <Order?> GetOrder(int id);
    Task <List<Order>> GetAllOrders();
    Task UpdateOrder(Order order);
    Task <List<Service>> GetServices(int orderId);
    Task UpdateOrderStatus(int orderId, Status statusEnam);
}