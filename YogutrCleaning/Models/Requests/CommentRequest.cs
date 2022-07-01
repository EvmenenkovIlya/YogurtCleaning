namespace YogurtCleaning.Models;

public class CommentRequest
{
    public string? Summary { get; set; }
    public int? ClientId { get; set; }
    public int? CleanerId { get; set; }
    public int OrderId { get; set; }
    public int Rating { get; set; }
}
