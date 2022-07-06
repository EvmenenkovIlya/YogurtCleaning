using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.DataLayer.Entities;

public class Order
{
    public int Id { get; set; }
    public CleaningObject CleaningObject { get; set; }
    public Status Status { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal Price { get; set; }
    public List<Service> Services { get; set; }
    public List<Cleaner>? CleanersBand { get; set; }
    public List<Comment> Comments { get; set; }
}
