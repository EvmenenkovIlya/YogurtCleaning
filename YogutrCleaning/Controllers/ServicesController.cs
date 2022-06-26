using Microsoft.AspNetCore.Mvc;

namespace YogutrCleaning.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly ILogger<ServicesController> _logger;

        public ServicesController(ILogger<ClientsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public Service GetService(int id)
        {
            return new Service();
        }

        [HttpGet]
        public List<Service> GetAllServices()
        {
            return new List<Service>();
        }

        [HttpPut("{id}/{name}/{price}/{unit}")]
        public void UpdateService(int id, string name, decimal price, string unit)
        {           
        }

        [HttpPost("{name}/{price}/{unit}")]
        public int AddService(string name, decimal price, string unit)
        {
            return new Service().Id;
        }

        [HttpDelete("{id}")]
        public int DeleteService(int id)
        {
            return new Service().Id;
        }
    }
}