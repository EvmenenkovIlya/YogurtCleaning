using System.ComponentModel.DataAnnotations;
using YogurtCleaning.Infrastructure;
namespace YogurtCleaning.Models;

public class ClientRegisterRequest
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.PasswordIsRequired)]
    [MinLength(8)]
    [MaxLength(50)]
    public string Password { get; set; }
    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    public string ConfirmPassword { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Phone { get; set; }
}
