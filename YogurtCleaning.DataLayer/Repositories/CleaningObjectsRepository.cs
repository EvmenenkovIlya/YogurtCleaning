using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.DataLayer.Repositories;

public class CleaningObjectsRepository :  ICleaningObjectsRepository
{
    private readonly YogurtCleaningContext _context;

    public CleaningObjectsRepository(YogurtCleaningContext context)
    {
        _context = context;
    }

    public async Task<int> CreateCleaningObject(CleaningObject cleaningObject)
    {
        _context.CleaningObjects.Add(cleaningObject);
        await _context.SaveChangesAsync();
        return cleaningObject.Id;
    }

    public async Task DeleteCleaningObject(CleaningObject cleaningObject)
    {
        cleaningObject.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<CleaningObject?> GetCleaningObject(int cleaningObjectId) => 
        await _context.CleaningObjects.FirstOrDefaultAsync(o => o.Id == cleaningObjectId);

    public async Task<List<CleaningObject>> GetAllCleaningObjects() =>
        await _context.CleaningObjects.Include(c => c.District).Include(c => c.Client).Where(o => o.IsDeleted == false).AsNoTracking().ToListAsync();

    public async Task<List<CleaningObject>> GetAllCleaningObjectsByClientId(int clientId) => 
        await _context.CleaningObjects.Include(c => c.District).Include(c => c.Client).Where(o => o.IsDeleted == false && o.Client.Id == clientId).AsNoTracking().ToListAsync();

    public async Task UpdateCleaningObject(CleaningObject modelToUpdate)
    {       
        _context.CleaningObjects.Update(modelToUpdate);
        await _context.SaveChangesAsync();
    }

    public async Task<District?> GetDistrict(DistrictEnum district) => await _context.Districts.FirstOrDefaultAsync(o => o.Id == district);
}