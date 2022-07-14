using System.ComponentModel.DataAnnotations;
using YogurtCleaning.Infrastructure;

namespace YogurtCleaning.Models
{ 
    public class CleaningObjectRequest
    {
        [Required(ErrorMessage = ApiErrorMessages.ClientIdIsRequred)]
        public int? ClientId { get; set; }
        [Required(ErrorMessage = ApiErrorMessages.NumberOfRoomsIsRequred)]
        public int? NumberOfRooms { get; set; }
        [Required(ErrorMessage = ApiErrorMessages.NumberOfBathroomsIsRequred)]
        public int? NumberOfBathrooms { get; set; }
        [Required(ErrorMessage = ApiErrorMessages.SquareIsRequred)]
        public int? Square { get; set; }
        [Required(ErrorMessage = ApiErrorMessages.NumberOfWindowsIsRequred)]
        public int? NumberOfWindows { get; set; }
        [Required(ErrorMessage = ApiErrorMessages.NumberOfBalconiesIsRequred)]
        public int? NumberOfBalconies { get; set; }
        [Required(ErrorMessage = ApiErrorMessages.AddressIsRequired)]
        [MaxLength(256, ErrorMessage = ApiErrorMessages.AddressMaxLength)]
        public string Address { get; set; }
    }
}
