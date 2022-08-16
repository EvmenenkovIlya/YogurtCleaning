namespace YogurtCleaning.DataLayer.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Summary { get; set; }
        public Client? Client { get; set; }
        public Cleaner? Cleaner { get; set; }
        public Order Order { get; set; }
        public int Rating { get; set; }
        public bool IsDeleted { get; set; }
    }
}
