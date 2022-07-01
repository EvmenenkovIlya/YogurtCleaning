using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers
{
    [ApiController]
    [AuthorizeRoles]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<ClientsController> _logger;

        public OrdersController(ILogger<ClientsController> logger)
        {
            _logger = logger;
        }

        [AuthorizeRoles(Role.Client, Role.Cleaner)]
        [HttpGet("{id}")]
        public ActionResult<OrderResponse> GetOrder(int id)
        {
            return Ok(new OrderResponse());
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