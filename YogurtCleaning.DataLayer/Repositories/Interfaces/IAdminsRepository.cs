using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories
{
    public interface IAdminsRepository
    {
        Task<Admin?> GetAdminByEmail(string email);
    }
}