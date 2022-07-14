using System.ComponentModel.DataAnnotations;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Infrastructure;

namespace YogurtCleaning.Models
{
    public class CleanerRegisterRequest
    {
        [Required(ErrorMessage = ApiErrorMessages.NameIsRequired)]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = ApiErrorMessages.LastNameIsRequired)]
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
        public List<int> ServicesIds { get; set; }
        public List<DistrictEnum> Districts { get; set; }
    }
}
