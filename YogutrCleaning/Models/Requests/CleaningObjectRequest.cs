using System.ComponentModel.DataAnnotations;
namespace YogurtCleaning.Models
{ 
    public class CleaningObjectRequest
    {
        [Required]
        public int ClientId { get; set; }
        [Required]
        public int NumberOfRooms { get; set; }
        [Required]
        public int NumberOfBathrooms { get; set; }
        [Required]
        public int Square { get; set; }
        [Required]
        public int NumberOfWindows { get; set; }
        [Required]
        public int NumberOfBalconies { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
