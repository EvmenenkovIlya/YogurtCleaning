using System.ComponentModel.DataAnnotations;
using YogurtCleaning.Infrastructure;

namespace YogurtCleaning.Models;

public class ClientUpdateRequest
{
    [Required(ErrorMessage = ApiErrorMessages.NameIsRequired)]
    public string Name { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.LastNameIsRequired)]
    public string LastName { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.BirthDateIsRequired)]
    public DateTime BirthDate { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.PhoneIsRequired)]
    public string Phone { get; set; }
}
