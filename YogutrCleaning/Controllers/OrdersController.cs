using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.Extensions;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IMapper _mapper;
    private readonly IOrdersService _ordersService;
    public OrdersController(IOrdersRepository ordersRepository, IMapper mapper, IOrdersService ordersService)
    {
        _ordersRepository = ordersRepository;
        _mapper = mapper;
        _ordersService = ordersService;
    }

    [AuthorizeRoles(Role.Client, Role.Cleaner)]
    [HttpGet("{orderId}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult<OrderResponse> GetOrder(int orderId)
    {
        var result = _ordersRepository.GetOrder(orderId);

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
        var result = _ordersRepository.GetAllOrders();
        return Ok(result);
    }

    [AuthorizeRoles(Role.Client)]
    [HttpPut("{orderId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult UpdateOrder([FromBody] OrderUpdateRequest order, int orderId)
    {
        _ordersService.UpdateOrder(_mapper.Map<Order>(order), orderId);
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
        int id = _ordersService.AddOrder(_mapper.Map<Order>(order));
        return Created($"{this.GetRequestFullPath()}/{id}", id);
    }

    [AuthorizeRoles]
    [HttpDelete("{orderId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult DeleteOrder(int orderId)
    {
        _ordersRepository.DeleteOrder(orderId);
        return NoContent();
    }

    [AuthorizeRoles(Role.Client, Role.Cleaner)]
    [HttpGet("{orderId}/services")]
    [ProducesResponseType(typeof(List<ServiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<List<ServiceResponse>> GetServices(int orderId)
    {

        return Ok(_ordersRepository.GetServices(orderId));
    }

    [AuthorizeRoles]
    [HttpPatch("{orderId}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult UpdateOrderStatus(int orderId, [FromBody] Status statusToUpdate)
    {
        _ordersRepository.UpdateOrderStatus(orderId, statusToUpdate);
        return NoContent();
    }

    [AuthorizeRoles(Role.Client, Role.Cleaner)]
    [HttpGet("{orderId}/CleaningObject")]
    [ProducesResponseType(typeof(CleaningObjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult<CleaningObjectResponse> GetCleaningObject(int orderId)
    {   
        var cleaningObject = _ordersRepository.GetCleaningObject(orderId);
        return Ok(new CleaningObjectResponse());
    }
}