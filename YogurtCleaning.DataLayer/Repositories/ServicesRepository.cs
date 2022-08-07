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
    public int AddService(Service service)
    {
        _context.Services.Add(service);
        _context.SaveChanges();

        return service.Id;
    }

    public void DeleteService(Service service)
    {
        service.IsDeleted = true;
        _context.SaveChanges();
    }

    public async Task<List<Service>> GetAllServices() => await _context.Services.Where(s => !s.IsDeleted).ToListAsync();

    public Service GetService(int id) => _context.Services.FirstOrDefault(s => s.Id == id && !s.IsDeleted);

    public void UpdateService(Service service)
    {
        _context.Services.Update(service);
        _context.SaveChanges();
    }
}