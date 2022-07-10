namespace YogurtCleaning.Models;

public class BundleResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal? PriceForRoom { get; set; }
    public decimal? PriceForBathroom { get; set; }
    public decimal? PriceForSquareMeter { get; set; }
    public List<ServiceResponse> Services { get; set; }
}
