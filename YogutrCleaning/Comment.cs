namespace YogutrCleaning
{
    public class Comment
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public int? ClientId { get; set; }
        public int? CleanerId { get; set; }
        public int OrderId { get; set; }
        public int Rating { get; set; }
    }
}
