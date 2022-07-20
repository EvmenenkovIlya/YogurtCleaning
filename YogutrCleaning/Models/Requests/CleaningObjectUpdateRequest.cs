using System.ComponentModel.DataAnnotations;
using YogurtCleaning.Infrastructure;

namespace YogurtCleaning.Models
{
    public class CleaningObjectUpdateRequest
    {
        [Range(0, int.MaxValue, ErrorMessage = ApiErrorMessages.NumberOfRoomsIsPositiveNumber)]
        public int NumberOfRooms { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = ApiErrorMessages.NumberOfBathroomsIsPositiveNumber)]
        public int NumberOfBathrooms { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = ApiErrorMessages.SquareIdIsPositiveNumber)]
        public int Square { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = ApiErrorMessages.NumberOfWindowsIdIsPositiveNumber)]
        public int NumberOfWindows { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = ApiErrorMessages.NumberOfBalconiesIdIsPositiveNumber)]
        public int NumberOfBalconies { get; set; }
        [Required(ErrorMessage = ApiErrorMessages.AddressIsRequired)]
        [MaxLength(256, ErrorMessage = ApiErrorMessages.AddressMaxLength)]
        public string Address { get; set; }
    }
}
