namespace YogurtCleaning.Models
{
    public class CleaningObjectResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public int Square { get; set; }
        public int NumberOfWindows { get; set; }
        public int NumberOfBalconies { get; set; }
        public string Address { get; set; }
    }
}
