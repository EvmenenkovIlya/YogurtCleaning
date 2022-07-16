using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.DataLayer.Entities;

public class Bundle
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Measure Measure { get; set; }
    public List<Service> Services { get; set; }
    public decimal Duration { get; set; } //in hours
    public bool IsDeleted { get; set; }
    public List<Order> Orders { get; set; }
}
