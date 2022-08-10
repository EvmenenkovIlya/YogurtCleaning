namespace YogurtCleaning.DataLayer.Entities;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Unit { get; set; }
    public decimal Duration { get; set; } //in hours
    public bool IsDeleted { get; set; }
    public List<Order> Orders { get; set; }
    public List<Cleaner> Cleaners { get; set; }
    public List<Bundle> Bundles { get; set; }
}
