using YogurtCleaning.Business.Models;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services;


public interface IOrdersService
{
    Task<Order?> GetOrder(int id, UserValues userValues);
    Task UpdateOrder(Order modelToUpdate, int id);
    Task DeleteOrder(int id, UserValues userValues);
    Task<List<Order>> GetAllOrders();
    Task<int> AddOrder(OrderBusinessModel order);
    Task<CleaningObject> GetCleaningObject(int orderId, UserValues userValues);
}