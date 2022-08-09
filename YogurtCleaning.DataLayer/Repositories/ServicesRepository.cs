using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public class ServicesRepository : IServicesRepository
{
    private readonly YogurtCleaningContext _context;
    public ServicesRepository(YogurtCleaningContext context)
    {
        _context = context;
    }
    public async Task<int> AddService(Service service)
    {
        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        return service.Id;
    }

    public async Task DeleteService(Service service)
    {
        service.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public  async Task<List<Service>> GetAllServices() => await _context.Services.Where(s => !s.IsDeleted).ToListAsync();

    public async Task<Service> GetService(int id) => await _context.Services.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

    public async Task UpdateService(Service service)
    {
         _context.Services.Update(service);
        await _context.SaveChangesAsync();
    }
}