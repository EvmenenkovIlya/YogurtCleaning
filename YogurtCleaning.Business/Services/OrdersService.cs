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

    public async Task DeleteOrder(int id, UserValues userValues)
    {
        var order = await _ordersRepository.GetOrder(id);
        Validator.CheckThatObjectNotNull(order, ExceptionsErrorMessages.OrderNotFound);
        AuthorizeEnitiyAccess(userValues, order);
        _ordersRepository.DeleteOrder(order);
    }

    public Order? GetOrder(int id, UserValues userValues)
    {
        throw new NotImplementedException();
    }

    public List<Order> GetAllOrders() => _ordersRepository.GetAllOrders();

    public async Task UpdateOrder(Order modelToUpdate, int id)
    {
        Order order = await _ordersRepository.GetOrder(id);

        order.Status = modelToUpdate.Status;
        order.StartTime = modelToUpdate.StartTime;
        order.UpdateTime = modelToUpdate.UpdateTime;
        order.Bundles = modelToUpdate.Bundles;
        order.Services = modelToUpdate.Services;
        order.CleanersBand = modelToUpdate.CleanersBand;
        _ordersRepository.UpdateOrder(order);
    }

    private void AuthorizeEnitiyAccess(UserValues userValues, Order order)
    {
        if (!(userValues.Id == order.Client.Id || userValues.Role == Role.Admin))
        {
            throw new AccessException($"Access denied");
        }
    }
}
