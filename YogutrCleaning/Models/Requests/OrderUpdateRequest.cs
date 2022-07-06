using YogurtCleaning.Enams;
namespace YogurtCleaning.Models;
public class OrderUpdateRequest
{
    public Status Status { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime UpdateTime { get; set; }
    public List<BundleResponse> Bundles { get; set; }
    public List<ServiceResponse> Services { get; set; }
    public List<CleanerResponse>? CleanersBand { get; set; }
}
