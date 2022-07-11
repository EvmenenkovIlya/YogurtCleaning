using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.DataLayer.Repositories.Intarfaces;
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
        private readonly IOrdersRepository _ordersRepository;

        public OrdersController(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        [AuthorizeRoles(Role.Client, Role.Cleaner)]
        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public ActionResult<OrderResponse> GetOrder(int orderId)
        {
            var result = _ordersRepository.GetOrderById(orderId);

            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [AuthorizeRoles]
        [HttpGet]
        [ProducesResponseType(typeof(List<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public ActionResult<List<OrderResponse>> GetAllOrders()
        {
            var result = _ordersRepository.GetOrders();
            return Ok(result);
        }

        [AuthorizeRoles(Role.Client)]
        [HttpPut("{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public ActionResult UpdateOrder([FromBody] OrderRequest order)
        {
            //_ordersRepository.UpdateOrder(order);
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
           // _ordersRepository.CreateOrder(order);
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
        [HttpPatch("{orderId-to-status}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public ActionResult UpdateOrderStatus(int orderId, int statusEnam)
        {
            return NoContent();
        }

        [AuthorizeRoles(Role.Client, Role.Cleaner)]
        [HttpGet("{orderId}/CleaningObject/{CleaningObjectId}")]
        [ProducesResponseType(typeof(CleaningObjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public ActionResult<CleaningObjectResponse> GetCleaningObject(int CleaningObjectId)
        {
            return Ok(new CleaningObjectResponse());
        }
    }
}