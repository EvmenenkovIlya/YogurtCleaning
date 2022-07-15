using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.Business.Services
{
    public interface IClientsService
    {
        void UpdateClient(Client newClient, int id);
    }
}