using System.ComponentModel.DataAnnotations;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Infrastructure;

namespace YogurtCleaning.Models;

public class ServiceRequest
{
    [Required(ErrorMessage = ApiErrorMessages.NameIsRequired)]
    [MaxLength(50, ErrorMessage = ApiErrorMessages.NameMaxLength)]
    public string Name { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.PriceIsRequired)]
    public decimal? Price { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.MeasureIsRequired)]
    public string? Unit { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.DurationIsRequired)]
    public decimal Duration { get; set; } //in hours
}
