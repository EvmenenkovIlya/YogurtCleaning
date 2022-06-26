namespace YogutrCleaning
{
    public class Packet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? PriceForRoom { get; set; }
        public decimal? PriceForBathroom { get; set; }
        public decimal? PriceForSquareMeter { get; set; }
        public List<Service> Services { get; set; }
    }
}
