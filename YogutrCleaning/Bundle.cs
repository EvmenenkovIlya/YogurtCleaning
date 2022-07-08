using YogurtCleaning.Enams;

namespace YogurtCleaning
{
    public class Bundle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Measure Measure { get; set; }
        public List<Service> Services { get; set; }
    }
}
