using Microsoft.AspNetCore.Mvc;

namespace YogutrCleaning.Controllers
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
        public Packet GetPacket(int id)
        {
            return new Packet();
        }
        [HttpGet]
        public List<Packet> GetAllPacketss()
        {
            return new List<Packet>();
        }
        [HttpPut("{id}/{name}/{priceForRoom}/{priceForBathroom}/{priceForSquareMeter}/{services}")]
        public void UpdatePacket(int id, string name, decimal priceForRoom, decimal priceForBathroom, decimal priceForSquareMeter, List<Service> services)
        {
        }
        [HttpPost("{name}/{priceForRoom}/{priceForBathroom}/{priceForSquareMeter}/{services}")]
        public int AddPacket(string name, decimal priceForRoom, decimal priceForBathroom, decimal priceForSquareMeter, List<Service> services)
        {
            return new Packet().Id;
        }

        [HttpDelete("{id}")]
        public int DeletePacket(int id)
        {
            return new Packet().Id;
        }
    }
}