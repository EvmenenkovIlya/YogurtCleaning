using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.Models;

public class OrderRequest
{
    public int CleaningObjectId { get; set; }
    public DateTime StartTime { get; set; }
    public List<BundleResponse> Bundles { get; set; }
    public List<ServiceResponse> Services { get; set; }
}
