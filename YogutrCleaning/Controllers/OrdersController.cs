using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Enams;
using YogurtCleaning.Extensions;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<ClientsController> _logger;

        public OrdersController(ILogger<ClientsController> logger)
        {
            _logger = logger;
        }

        [AuthorizeRoles(Role.Client, Role.Cleaner)]
        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public ActionResult<OrderResponse> GetOrder(int orderId)
        {
            return Ok(new OrderResponse());
        }

        [AuthorizeRoles]
        [HttpGet]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public ActionResult<List<OrderResponse>> GetAllOrders()
        {
            return Ok(new List<OrderResponse>());
        }

        [AuthorizeRoles(Role.Client)]
        [HttpPut("{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public ActionResult UpdateOrder([FromBody] OrderRequest order)
        {
            return NoContent();
        }

        [AuthorizeRoles(Role.Client)]
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<int> AddOrder(OrderRequest order)
        {
            var orderCreated = new OrderResponse() { Id = 5 };
            return Created($"{this.GetRequestFullPath()}/{orderCreated.Id}", orderCreated.Id);
        }

        [AuthorizeRoles]
        [HttpDelete("{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public ActionResult DeleteOrder(int orderId)
        {
            return NoContent();
        }

        [AuthorizeRoles(Role.Client, Role.Cleaner)]
        [HttpGet("{orderId}/services")]
        [ProducesResponseType(typeof(List<ServiceResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public ActionResult<List<ServiceResponse>> GetServices(int orderId)
        {
            return Ok(new List<ServiceResponse>());
        }

        [AuthorizeRoles]
        [HttpPatch("{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public ActionResult UpdateOrderStatus(int orderId, [FromBody] OrderUpdateRequest orderUpdateRequest)
        {
            return NoContent();
        }

        [AuthorizeRoles(Role.Client, Role.Cleaner)]
        [HttpGet("{orderId}/CleaningObject/{CleaningObjectId}")]
        [ProducesResponseType(typeof(CleaningObjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public ActionResult<CleaningObject> GetCleaningObject(int CleaningObjectId)
        {
            return Ok(new CleaningObjectResponse());
        }
    }
}