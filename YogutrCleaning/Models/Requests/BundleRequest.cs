using System.ComponentModel.DataAnnotations;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Infrastructure;

namespace YogurtCleaning.Models;

public class BundleRequest
{
    [Required(ErrorMessage = ApiErrorMessages.NameIsRequired)]
    [MaxLength(100, ErrorMessage = ApiErrorMessages.BundleNameMaxLenght)]
    public string Name { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.PriceIsRequired)]
    public decimal? Price { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.MeasureIsRequired)]
    public Measure? Measure { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.ServicesIsRequired)]
    public List<ServiceResponse> Services { get; set; }
}