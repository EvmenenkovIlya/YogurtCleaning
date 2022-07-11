using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.Models;

public class ServiceResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Measure Measure { get; set; }
}
