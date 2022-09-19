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

    public async Task <Order?> GetOrder(int orderId) => await _context.Orders
        .Include(o => o.Client)
        .Include(o => o.CleaningObject)
        .Include(o => o.CleanersBand)
        .Include(o => o.Bundles)
        .Include(o => o.Services)
        .FirstOrDefaultAsync(o => o.Id == orderId);

    public async Task <List<Order>> GetAllOrders() => await _context.Orders.AsNoTracking().Where(o => !o.IsDeleted).ToListAsync();

    public async Task <int> CreateOrder(Order order)
    {
        order.UpdateTime = DateTime.Now;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        
        return order.Id;
    }

    public async Task DeleteOrder(Order order)
    {
        order.IsDeleted = true;
        order.UpdateTime = DateTime.Now;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrder(Order modelToUpdate)
    {
        modelToUpdate.UpdateTime = DateTime.Now;
        _context.Orders.Update(modelToUpdate);
        modelToUpdate.UpdateTime = DateTime.Now;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateOrderStatus(int orderId, Status statusToUpdate)
    {
        var order = await GetOrder(orderId);
        order.Status = statusToUpdate;
        order.UpdateTime = DateTime.Now;
        await _context.SaveChangesAsync();
    }
}
