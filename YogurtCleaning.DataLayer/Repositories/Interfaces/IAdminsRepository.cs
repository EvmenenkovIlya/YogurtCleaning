using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories
{
    public interface IAdminsRepository
    {
        Admin? GetAdminByEmail(string email);
    }
}