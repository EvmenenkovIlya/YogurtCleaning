using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer.Repositories;

public class BundlesRepository : IBundlesRepository
{
    private readonly YogurtCleaningContext _context;
    public BundlesRepository(YogurtCleaningContext context)
    {
        _context = context;
    }
    public int AddBundle(Bundle bundle)
    {
        _context.Bundles.Add(bundle);
        _context.SaveChanges();

        return bundle.Id;
    }

    public void DeleteBundle(int id)
    {
        var bundle = _context.Bundles.FirstOrDefault(b => b.Id == id);
        bundle.IsDeleted = true;
        _context.SaveChanges();
    }

    public List<Bundle> GetAllBundles() => _context.Bundles.Where(b => !b.IsDeleted).ToList();

    public Bundle GetBundle(int id) => _context.Bundles.FirstOrDefault(b => b.Id == id && !b.IsDeleted);

    public void UpdateBundle(Bundle bundle, int id)
    {
        _context.Bundles.Update(bundle);
        _context.SaveChanges();
    }
}
