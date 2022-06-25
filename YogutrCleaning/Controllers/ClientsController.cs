using Microsoft.AspNetCore.Mvc;

namespace YogutrCleaning.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(ILogger<ClientsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public Client GetClient(int id)
        {
            return new Client();
        }
        [HttpGet]
        public List<Client> GetAllClients()
        {
            return new List<Client>();
        }
        [HttpPut("{id}")]
        public Client UpdateClient(int id)
        {
            return new Client();
        }
        [HttpPost()]
        public int AddClient()
        {
            return new Client().Id;
        }

        [HttpDelete("{id}")]
        public int DeleteClient(int id)
        {
            return new Client().Id;
        }
    }
}