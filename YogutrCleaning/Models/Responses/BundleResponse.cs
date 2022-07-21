using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.Models;

public class BundleResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CleaningType Type { get; set; }
    public decimal Price { get; set; }
    public Measure Measure { get; set; }
    public List<ServiceResponse> Services { get; set; }
    public decimal Duration { get; set; } //in hours
}
