using Microsoft.AspNetCore.Mvc;

namespace YogurtCleaning.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<ClientsController> _logger;

        public OrdersController(ILogger<ClientsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public Order GetOrder(int id)
        {
            return new Order();
        }
        [HttpGet]
        public List<Order> GetAllOrders()
        {
            return new List<Order>();
        }
        [HttpPut("{id}")]
        public void UpdateOrder(int id)
        {           
        }
        [HttpPost()]
        public int AddOrder()
        {
            return new Order().Id;
        }

        [HttpDelete("{id}")]
        public int DeleteOrder(int id)
        {
            return new Order().Id;
        }
        [HttpGet("{id}/services")]
        public List<Service> GetServices(int id)
        {
            return new List<Service>();
        }
    }
}