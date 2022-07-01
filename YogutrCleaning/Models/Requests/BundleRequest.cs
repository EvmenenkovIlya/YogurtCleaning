namespace YogurtCleaning.Models;

public class BundleRequest
{
    public string Name { get; set; }
    public decimal? PriceForRoom { get; set; }
    public decimal? PriceForBathroom { get; set; }
    public decimal? PriceForSquareMeter { get; set; }
    public List<ServiceResponce> Services { get; set; }
}
