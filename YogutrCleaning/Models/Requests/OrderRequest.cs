namespace YogurtCleaning.Models;

public class OrderRequest
{
    public int ClientId { get; set; }
    public int CleaningObjectId { get; set; }
    public DateTime StartTime { get; set; }
    public List<int> BundlesIds { get; set; }
    public List<int> ServicesIds { get; set; }
}
