using YogurtCleaning.Business.Models;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.Business.Services;


public interface IOrdersService
{
    Task<Order?> GetOrder(int id, UserValues userValues);
    Task<List<Service>> GetOrderServices(int id, UserValues userValues);
    Task UpdateOrder(OrderBusinessModel modelToUpdate, int id, UserValues userValues);
    Task UpdateOrderStatus(int orderId, Status status); 
    Task UpdateOrderPaymentStatus(int orderId, PaymentStatus paymentStatus);
    Task DeleteOrder(int id, UserValues userValues);
    Task<List<Order>> GetAllOrders();
    Task<int> AddOrder(OrderBusinessModel order);
    Task<CleaningObject> GetCleaningObject(int orderId, UserValues userValues);
}