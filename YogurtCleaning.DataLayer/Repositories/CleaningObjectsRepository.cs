using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public class CleaningObjectsRepository :  ICleaningObjectsRepository
{
    private readonly YogurtCleaningContext _context;

    public CleaningObjectsRepository(YogurtCleaningContext context)
    {
        _context = context;
    }

    public int CreateCleaningObject(CleaningObject cleaningObject)
    {
        _context.CleaningObjects.Add(cleaningObject);
        _context.SaveChanges();
        return cleaningObject.Id;
    }

    public void DeleteCleaningObject(int cleaningObjectId)
    {
        var cleaningObject = GetCleaningObject(cleaningObjectId);
        cleaningObject.IsDeleted = true;
        _context.SaveChanges();
    }

    public CleaningObject? GetCleaningObject(int cleaningObjectId) => 
        _context.CleaningObjects.FirstOrDefault(o => o.Id == cleaningObjectId);

    public List<CleaningObject> GetAllCleaningObjects() => 
        _context.CleaningObjects.AsNoTracking().Where(o => !o.IsDeleted).ToList();

    public void UpdateCleaningObject(CleaningObject modelToUpdate)
    {       
        _context.CleaningObjects.Update(modelToUpdate);
        _context.SaveChanges();
    }
}