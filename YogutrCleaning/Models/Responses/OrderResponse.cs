using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.Models;

public class OrderResponse
{
    public int Id { get; set; }
    public int CleaningObjectId { get; set; }
    public Status Status { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime UpdateTime { get; set; }
    public decimal Price { get; set; }
    public List<BundleResponse> Bundles { get; set; }
    public List<ServiceResponse> Services { get; set; }
    public List<CleanerResponse>? CleanersBand { get; set; }
    public List<CommentResponse> Comments { get; set; }
}
