namespace YogutrCleaning
{
    public class Order
    {
        public int Id { get; set; }
        public int CleaningObjectId { get; set; }
        public string Status { get; set; } // Must be ENAM in future
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Addresses { get; set; }
        public decimal Price { get; set; }
        public List<Service> Services;
        //public List<Cleaners> ?
        //public Comment
    }
}