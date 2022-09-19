using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public class AdminsRepository : IAdminsRepository
{
    private readonly YogurtCleaningContext _context;

    public AdminsRepository(YogurtCleaningContext context)
    {
        _context = context;
    }

    public async Task<Admin?> GetAdminByEmail(string email) => await _context.Admins.FirstOrDefaultAsync(admin => admin.Email == email);
}
