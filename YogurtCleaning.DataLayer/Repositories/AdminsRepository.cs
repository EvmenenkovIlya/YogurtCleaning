using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public class AdminsRepository : IAdminsRepository
{
    private readonly YogurtCleaningContext _context;

    public AdminsRepository(YogurtCleaningContext context)
    {
        _context = context;
    }

    public Admin? GetAdminByEmail(string email) => _context.Admins.FirstOrDefault(admin => admin.Email == email);
}
