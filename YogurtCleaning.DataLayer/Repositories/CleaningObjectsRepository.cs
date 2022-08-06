﻿using Microsoft.EntityFrameworkCore;
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

    public int CreateCleaningObject(CleaningObject cleaningObject)
    {
        _context.CleaningObjects.Add(cleaningObject);
        _context.SaveChanges();
        return cleaningObject.Id;
    }

    public void DeleteCleaningObject(CleaningObject cleaningObject)
    {
        cleaningObject.IsDeleted = true;
        _context.SaveChanges();
    }

    public CleaningObject? GetCleaningObject(int cleaningObjectId) => 
        _context.CleaningObjects.Include(x  => x.Client).Include(x => x.District).FirstOrDefault(o => o.Id == cleaningObjectId);

    public List<CleaningObject> GetAllCleaningObjects() => 
        _context.CleaningObjects.Where(o => o.IsDeleted == false).AsNoTracking().ToList();

    public void UpdateCleaningObject(CleaningObject modelToUpdate)
    {       
        _context.CleaningObjects.Update(modelToUpdate);
        _context.SaveChanges();
    }

    public District GetDistrict(DistrictEnum district) => _context.Districts.FirstOrDefault(o => o.Id == district);
}