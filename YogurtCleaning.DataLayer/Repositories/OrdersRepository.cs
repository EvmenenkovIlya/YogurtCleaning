using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories.Intarfaces;

namespace YogurtCleaning.DataLayer.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly YogurtCleaningContext _context;

    public OrdersRepository(YogurtCleaningContext context)
    {
        _context = context;
    }

    public Order? GetOrderById(int id) =>
        _context.Orders           
            .FirstOrDefault(o => o.Id == id && !o.IsDeleted);

    public List<Order> GetOrders() => _context.Orders.AsNoTracking().Where(o => o.StartTime > DateTime.Now.AddDays(-5) && !o.IsDeleted).ToList();

    public int CreateOrder(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
        
        return order.Id;
    }

    public void DeleteOrder(int id)
    {
        var order = _context.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
            return;
        else
        {
            order.IsDeleted = true;
            order.UpdateTime = DateTime.Now;
            _context.SaveChanges();
        }
    }

    public void UpdateOrder(Order order)
    {
        _context.Orders.Update(order);
        _context.SaveChanges();
    }

    public List<Service> GetServices(int orderId)
    {
        var order = GetOrderById(orderId);
        if (order == null)
        {
            return null;
        }
        else
        {
            return order.Services;
        }
    }

    public void UpdateOrderStatus(int orderId, int statusEnam)
    {
        var order = GetOrderById(orderId);
        //update order
        throw new NotImplementedException();
    }

    public CleaningObject GetCleaningObject(int orderId, int CleaningObjectId)
    {
        var order = GetOrderById(orderId);
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
