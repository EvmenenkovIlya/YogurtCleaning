using PetProject.DataLayer.Entities;

namespace PetProject.DataLayer.Repositories
{
    public interface IOrdersRepository
    {
        Order? GetOrderById(int id);
        int CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int id);
    }
}