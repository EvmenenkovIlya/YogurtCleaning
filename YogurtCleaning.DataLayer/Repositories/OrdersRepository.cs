using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories.Intarfaces;

namespace YogurtCleaning.DataLayer.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly YogurtCleaningContext _context;

    public OrdersRepository(YogurtCleaningContext context)
    {
        _context = context;
    }

    public Order? GetOrder(int orderId) =>
        _context.Orders           
            .FirstOrDefault(o => o.Id == orderId && !o.IsDeleted);

    public List<Order> GetOrders() => _context.Orders.AsNoTracking().Where(o => !o.IsDeleted).ToList();

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

    public void UpdateOrder(Order newProperty)
    {
        var order = GetOrder(newProperty.Id);

        order.Status = newProperty.Status;
        order.StartTime = newProperty.StartTime;
        order.UpdateTime = newProperty.UpdateTime;
        order.Bundles = newProperty.Bundles;
        order.Services = newProperty.Services;
        order.CleanersBand = newProperty.CleanersBand;
    
        _context.Orders.Update(order);
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

    public void UpdateOrderStatus(int orderId, int statusEnam)
    {
        var order = GetOrder(orderId);
        order.Status = (Status)Enum.ToObject(typeof(Status), statusEnam);
        _context.SaveChanges();
    }

    public CleaningObject GetCleaningObject(int orderId, int CleaningObjectId)
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
