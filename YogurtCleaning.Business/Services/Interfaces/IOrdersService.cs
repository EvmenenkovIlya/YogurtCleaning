using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services;

public interface IOrdersService
{
    void UpdateOrder(Order modelToUpdate, int id);
    int AddOrder(Order order);
}