using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Business;
using YogurtCleaning.Business.Models;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Extensions;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IOrdersService _ordersService;
    public UserValues? _userValues;
    public OrdersController(IMapper mapper, IOrdersService ordersService)
    {
        _mapper = mapper;
        _ordersService = ordersService;
    }

    [AuthorizeRoles(Role.Client, Role.Cleaner)]
    [HttpGet("{orderId}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task <ActionResult<OrderResponse>> GetOrder(int orderId)
    {
        _userValues = this.GetClaimsValue();
        var result = await _ordersService.GetOrder(orderId, _userValues);
        return Ok(_mapper.Map<OrderResponse>(result));
    }

    [AuthorizeRoles]
    [HttpGet]
    [ProducesResponseType(typeof(List<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<OrderResponse>>> GetAllOrders()
    {
        var orders = await _ordersService.GetAllOrders();
        return Ok(_mapper.Map<List<OrderResponse>>(orders));
    }

    [AuthorizeRoles(Role.Client)]
    [HttpPut("{orderId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> UpdateOrder([FromBody] OrderUpdateRequest order, int orderId)
    {
        _userValues = this.GetClaimsValue();
        await _ordersService.UpdateOrder(_mapper.Map<OrderBusinessModel>(order), orderId, _userValues);
        return NoContent();
    }

    [AuthorizeRoles(Role.Client)]
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<int>> AddOrder(OrderRequest order)
    {
        _userValues = this.GetClaimsValue();
        int id = await _ordersService.AddOrder(_mapper.Map<OrderBusinessModel>(order), _userValues);
        return Created($"{this.GetRequestFullPath()}/{id}", id);
    }

    [AuthorizeRoles]
    [HttpDelete("{orderId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> DeleteOrder(int orderId)
    {
        _userValues = this.GetClaimsValue();
        await _ordersService.DeleteOrder(orderId, _userValues);
        return NoContent();
    }

    [AuthorizeRoles(Role.Client, Role.Cleaner)]
    [HttpGet("{orderId}/services")]
    [ProducesResponseType(typeof(List<ServiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<ServiceResponse>>> GetServices(int orderId)
    {
        _userValues = this.GetClaimsValue();
        var services = await _ordersService.GetOrderServices(orderId, _userValues);
        return Ok(_mapper.Map<List<ServiceResponse>>(services));
    }

    [AuthorizeRoles]
    [HttpPatch("{orderId}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> UpdateOrderStatus(int orderId, [FromBody] Status statusToUpdate)
    {
        await _ordersService.UpdateOrderStatus(orderId, statusToUpdate);
        return NoContent();
    }

    [AuthorizeRoles]
    [HttpPatch("{orderId}/payment-status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> UpdateOrderPaymentStatus(int orderId, [FromBody] PaymentStatus statusToUpdate)
    {
        await _ordersService.UpdateOrderPaymentStatus(orderId, statusToUpdate);
        return NoContent();
    }

    [AuthorizeRoles(Role.Client, Role.Cleaner)]
    [HttpGet("{orderId}/cleaning-object")]
    [ProducesResponseType(typeof(CleaningObjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CleaningObjectResponse>> GetCleaningObject(int orderId)
    {
        _userValues = this.GetClaimsValue();
        var cleaningObject = await _ordersService.GetCleaningObject(orderId, _userValues);
        return Ok(_mapper.Map<CleaningObjectResponse>(cleaningObject));
    }
}