using YogurtCleaning.Enams;
namespace YogurtCleaning.Models;
public class OrderUpdateRequest
{
    public Status Status { get; set; }
    public DateTime UpdateTime { get; set; }
}
