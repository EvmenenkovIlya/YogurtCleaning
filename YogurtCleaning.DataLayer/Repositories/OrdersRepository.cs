using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.DataLayer.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly YogurtCleaningContext _context;

    public OrdersRepository(YogurtCleaningContext context)
    {
        _context = context;
    }

    public Order? GetOrder(int orderId) => _context.Orders.FirstOrDefault(o => o.Id == orderId);

    public List<Order> GetAllOrders() => _context.Orders.AsNoTracking().Where(o => !o.IsDeleted).ToList();

    public int CreateOrder(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
        
        return order.Id;
    }

    public void DeleteOrder(int orderId)
    {
        var order = GetOrder(orderId);
        if (order == null)
            return;
        else
        {
            order.IsDeleted = true;
            order.UpdateTime = DateTime.Now;
            _context.SaveChanges();
        }
    }

    public void UpdateOrder(Order modelToUpdate)
    {
        _context.Orders.Update(modelToUpdate);
        _context.SaveChanges();
    }

    public List<Service> GetServices(int orderId)
    {
        var order = GetOrder(orderId);
        if (order == null)
        {
            return null;
        }
        else
        {
            return order.Services;
        }
    }

    public void UpdateOrderStatus(int orderId, Status statusToUpdate)
    {
        var order = GetOrder(orderId);
        order.Status = statusToUpdate;
        _context.SaveChanges();
    }

    public CleaningObject GetCleaningObject(int orderId)
    {
        var order = GetOrder(orderId);
        if (order == null)
        {
            return null;
        }
        else
        {
            return order.CleaningObject;
        }
    }
}
