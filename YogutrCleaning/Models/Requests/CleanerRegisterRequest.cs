using System.ComponentModel.DataAnnotations;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Infrastructure;

namespace YogurtCleaning.Models;

public class CleanerRegisterRequest
{
    [Required(ErrorMessage = ApiErrorMessages.NameIsRequired)]
    [MaxLength(50, ErrorMessage = ApiErrorMessages.NameMaxLength)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = ApiErrorMessages.LastNameIsRequired)]
    [MaxLength(50, ErrorMessage = ApiErrorMessages.LastNameMaxLength)]
    public string LastName { get; set; }

    [Required(ErrorMessage = ApiErrorMessages.BirthDateIsRequired)]
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = ApiErrorMessages.PasswordIsRequired)]
    [MinLength(8, ErrorMessage = ApiErrorMessages.PasswordMinLength)]
    [MaxLength(50, ErrorMessage = ApiErrorMessages.PasswordMaxLength)]
    public string Password { get; set; }

    [Required(ErrorMessage = ApiErrorMessages.ConfirmPasswordIsRequired)]
    [MinLength(8, ErrorMessage = ApiErrorMessages.ConfirmPasswordMinLength)]
    [MaxLength(50, ErrorMessage = ApiErrorMessages.ConfirmPasswordMaxLength)]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = ApiErrorMessages.EmailIsRequired)]
    [EmailAddress(ErrorMessage = ApiErrorMessages.EmailNotValid)]
    [MaxLength(255, ErrorMessage = ApiErrorMessages.EmailMaxLength)]
    public string Email { get; set; }

    [Required(ErrorMessage = ApiErrorMessages.PhoneIsRequired)]
    [MaxLength(15, ErrorMessage = ApiErrorMessages.PhoneMaxLength)]
    public string Phone { get; set; }


    [Required(ErrorMessage = ApiErrorMessages.PassportIsRequired)]
    [MinLength(10, ErrorMessage = ApiErrorMessages.PassportLength)]
    [MaxLength(10, ErrorMessage = ApiErrorMessages.PassportLength)]
    public string Passport { get; set; }

    [Required(ErrorMessage = ApiErrorMessages.ScheduleIsRequired)]
    public Schedule Schedule { get; set; }
        public List<int> ServicesIds { get; set; }
        public List<DistrictEnum> Districts { get; set; }
}