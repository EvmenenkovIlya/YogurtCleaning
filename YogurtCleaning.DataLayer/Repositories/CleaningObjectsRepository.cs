using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories.Intarfaces;

namespace YogurtCleaning.DataLayer.Repositories;

public class CleaningObjectsRepository : ICleaningObjectsRepository
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

    CleaningObject? GetCleaningObject(int cleaningObjectId)
    {
        return _context.CleaningObjects.FirstOrDefault(o => o.Id == cleaningObjectId && !o.IsDeleted);
    }

    public List<CleaningObject> GetAllCleaningObjects()
    {
        return _context.CleaningObjects.AsNoTracking().Where(o => !o.IsDeleted).ToList<CleaningObject>();
    }

     public void UpdateCleaningObject(CleaningObject newProperty)
    {  
        var cleaningObject = GetCleaningObject(newProperty.Id);

        cleaningObject.NumberOfRooms = newProperty.NumberOfRooms;
        cleaningObject.NumberOfBathrooms = newProperty.NumberOfBathrooms;
        cleaningObject.NumberOfWindows = newProperty.NumberOfWindows;
        cleaningObject.NumberOfBalconies = newProperty.NumberOfBalconies;
        cleaningObject.Address = newProperty.Address;
        cleaningObject.Square = newProperty.Square;

        _context.CleaningObjects.Update(cleaningObject);
        _context.SaveChanges();
    }
}