using System.ComponentModel.DataAnnotations;

namespace YogurtCleaning.Models;

public class UserLoginRequest
{
    [Required]
    public string Password { get; set; }
    [Required]
    public string Email { get; set; }
}
