using YogurtCleaning.Business.Exceptions;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;

namespace YogurtCleaning.Business.Services;

public class OrdersService : IOrdersService
{
    private readonly IOrdersRepository _ordersRepository;

    public OrdersService(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public void DeleteOrder(int id, UserValues userValues)
    {
        var order = _ordersRepository.GetOrder(id);
        Validator.CheckThatObjectNotNull(order, ExceptionsErrorMessages.OrderNotFound);
        AuthorizeEnitiyAccess(userValues, order);
        _ordersRepository.DeleteOrder(order);
    }

    public Order? GetOrder(int id, UserValues userValues)
    {
        return _ordersRepository.GetOrder(id);
    }

    public List<Order> GetAllOrders() => _ordersRepository.GetAllOrders();

    public void UpdateOrder(Order modelToUpdate, int id)
    {
        Order order = _ordersRepository.GetOrder(id);
        Validator.CheckThatObjectNotNull(order, ExceptionsErrorMessages.OrderNotFound);
        order!.Status = modelToUpdate.Status;
        order.StartTime = modelToUpdate.StartTime;
        order.UpdateTime = modelToUpdate.UpdateTime;
        order.Bundles = modelToUpdate.Bundles;
        order.Services = modelToUpdate.Services;
        order.CleanersBand = modelToUpdate.CleanersBand;
        _ordersRepository.UpdateOrder(order);
    }

    public CleaningObject GetCleaningObject(int orderId, UserValues userValues)
    {
        var order = GetOrder(orderId, userValues);
        Validator.CheckThatObjectNotNull(order, ExceptionsErrorMessages.OrderNotFound);
        if (!(userValues.Id == order.Client.Id || userValues.Role == Role.Admin || 
            (order.CleanersBand.Find(c => c.Id == userValues.Id) != null) && userValues.Role == Role.Cleaner))
        {
            throw new AccessException($"Access denied");
        }
        return order.CleaningObject;
    }

    private void AuthorizeEnitiyAccess(UserValues userValues, Order order)
    {
        if (!(userValues.Id == order.Client.Id || userValues.Role == Role.Admin))
        {
            throw new AccessException($"Access denied");
        }
    }
}
