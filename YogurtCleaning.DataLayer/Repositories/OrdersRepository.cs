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

    public async Task<Order?> GetOrder(int orderId) => await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

    public List<Order> GetAllOrders() => _context.Orders.AsNoTracking().Where(o => !o.IsDeleted).ToList();

    public int CreateOrder(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
        
        return order.Id;
    }

    public void DeleteOrder(Order order)
    {
        order.IsDeleted = true;
        order.UpdateTime = DateTime.Now;
        _context.SaveChanges();
    }

    public void UpdateOrder(Order modelToUpdate)
    {
        _context.Orders.Update(modelToUpdate);
        _context.SaveChanges();
    }

    public async Task<List<Service>> GetServices(int orderId)
    {
        var order = await GetOrder(orderId);
        if (order == null)
        {
            return null;
        }
        else
        {
            return order.Services;
        }
    }

    public async Task UpdateOrderStatus(int orderId, Status statusToUpdate)
    {
        var order = await GetOrder(orderId);
        order.Status = statusToUpdate;
        _context.SaveChanges();
    }

    public async Task<CleaningObject> GetCleaningObject(int orderId)
    {
        var order = await GetOrder(orderId);
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
