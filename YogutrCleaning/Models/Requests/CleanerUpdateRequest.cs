namespace YogurtCleaning.Models.Requests
{
    public class CleanerUpdateRequest
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
    }
}
