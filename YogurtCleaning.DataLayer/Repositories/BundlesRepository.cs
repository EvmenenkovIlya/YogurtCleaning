
using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public class BundlesRepository : IBundlesRepository
{
    private readonly YogurtCleaningContext _context;
    public BundlesRepository(YogurtCleaningContext context)
    {
        _context = context;
    }
    public async Task<int> AddBundle(Bundle bundle)
    {
        _context.Bundles.Add(bundle);
        await _context.SaveChangesAsync();

        return bundle.Id;
    }

    public async Task DeleteBundle(Bundle bundle)
    {       
        bundle.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Bundle>> GetAllBundles() => await _context.Bundles.Where(b => !b.IsDeleted).ToListAsync();

    public async Task<Bundle> GetBundle(int id) => await _context.Bundles.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted)!;

    public async Task UpdateBundle(Bundle bundle)
    {
        _context.Bundles.Update(bundle);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Service>> GetServices(List<Service> services)
    {
        List<int> servicesIds = services.Select(s => s.Id).ToList();

        return await _context.Services.Where(c => servicesIds.Contains(c.Id)).ToListAsync();
    }
}