using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YogurtCleaning.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PacketController : ControllerBase
    {
        private readonly ILogger<PacketController> _logger;

        public PacketController(ILogger<PacketController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public Bundle GetBundle(int id)
        {
            return new Bundle();
        }

        [HttpGet]
        public List<Bundle> GetAllBundles()
        {
            return new List<Bundle>();
        }
        [HttpPut("{id}/{name}/{priceForRoom}/{priceForBathroom}/{priceForSquareMeter}/{services}")]
        public void UpdateBundle(int id, string name, decimal priceForRoom, decimal priceForBathroom, decimal priceForSquareMeter, List<Service> services)
        {
        }
        [HttpPost("{name}/{priceForRoom}/{priceForBathroom}/{priceForSquareMeter}/{services}")]
        public int AddBundle(string name, decimal priceForRoom, decimal priceForBathroom, decimal priceForSquareMeter, List<Service> services)
        {
            return new Bundle().Id;
        }

        [HttpDelete("{id}")]
        public int DeleteBundle(int id)
        {
            return new Bundle().Id;
        }
    }
}