using System.ComponentModel.DataAnnotations;
using YogurtCleaning.Infrastructure;

namespace YogurtCleaning.Models;

public class CommentRequest
{
    [MaxLength(500, ErrorMessage = ApiErrorMessages.SummaryMaxLenght)]
    public string? Summary { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = ApiErrorMessages.OrderIdIsRequred)]
    public int OrderId { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.RatingIsRequred)]
    [Range(1, 5, ErrorMessage = ApiErrorMessages.RatingIsOutOfRange)]
    public int Rating { get; set; }
}
