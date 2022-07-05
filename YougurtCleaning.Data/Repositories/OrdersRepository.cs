using Microsoft.EntityFrameworkCore;
using YougurtCleaning;

namespace PetProject.DataLayer.Repositories;

public class OrdersRepository// : IOrdersRepository
{
    //private readonly PetProjectContext _context;

    //public OrdersRepository(PetProjectContext context)
    //{
    //    _context = context;
    //}

    //public Order? GetOrderById(int id) => 
    //    _context.Orders
    //        .Include(o => o.OrderDetails)
    //            .ThenInclude(od => od.Product)
    //        .FirstOrDefault(o => o.Id == id);

    //public List<Order> GetOrders() => _context.Orders.Where(o => o.OrderDate > DateTime.Now.AddDays(-5)).ToList();

    //public int CreateOrder(Order order)
    //{
    //    _context.Orders.Add(order);
    //    _context.SaveChanges();

    //    return order.Id;
    //}

    //public void DeleteOrder(int id)
    //{
    //    _context.Orders.Remove(new Order { Id = 2 });
    //    _context.SaveChanges();
    //}

    //public void UpdateOrder(Order order)
    //{
    //    _context.Orders.Update(order);
    //    _context.SaveChanges();
    //}
}
