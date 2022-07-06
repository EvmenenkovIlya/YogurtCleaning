using YogurtCleaning.DataLayer.Enums;

namespace YogurtCleaning.DataLayer.Entities
{
    public class CleaningObject
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public int Square { get; set; }
        public int NumberOfWindows { get; set; }
        public int NumberOfBalconies { get; set; }
        public string Address { get; set; }
        public District District { get; set; }
        public List<Order> Orders { get; set; }
    }
}
