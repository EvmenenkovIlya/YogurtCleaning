using System.ComponentModel.DataAnnotations;
using YogurtCleaning.Infrastructure;

namespace YogurtCleaning.Models
{ 
    public class CleaningObjectRequest : CleaningObjectUpdateRequest
    {
        [Range(1,int.MaxValue, ErrorMessage = ApiErrorMessages.ClientIdIsPositiveNumber)]
        public int ClientId { get; set; }
    }
}
