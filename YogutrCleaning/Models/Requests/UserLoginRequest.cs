using System.ComponentModel.DataAnnotations;
using YogurtCleaning.Infrastructure;

namespace YogurtCleaning.Models;

public class UserLoginRequest
{
    [Required(ErrorMessage = ApiErrorMessages.PasswordIsRequired)]
    public string Password { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.EmailIsRequired)]
    [EmailAddress(ErrorMessage = ApiErrorMessages.EmailNotValid)]
    public string Email { get; set; }
    public bool IsCleaner { get; set; } // for auth
}
