using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Business;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Extensions;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class CleanersController : ControllerBase
{
    private readonly IMapper _mapper;
    public UserValues userValues;
    private readonly ICleanersService _cleanersService;

    public CleanersController(IMapper mapper, ICleanersService cleanersService)
    {
        _mapper = mapper;
        _cleanersService = cleanersService;
    }

    [AuthorizeRoles(Role.Cleaner)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CleanerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult<CleanerResponse> GetCleaner(int id)
    {
        userValues = this.GetClaimsValue();
        var cleaner = _cleanersService.GetCleaner(id, userValues);
        return Ok(_mapper.Map<CleanerResponse>(cleaner));
    }

    [AuthorizeRoles]
    [HttpGet]
    [ProducesResponseType(typeof(List<CleanerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<List<CleanerResponse>> GetAllCleaners()
    {
        var cleaners = _cleanersService.GetAllCleaners();
        return Ok(_mapper.Map<List<CleanerResponse>>(cleaners));
    }

    [AuthorizeRoles(Role.Cleaner)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult UpdateCleaner([FromBody] CleanerUpdateRequest cleaner, int id)
    {
        userValues = this.GetClaimsValue();
        _cleanersService.UpdateCleaner(_mapper.Map<Cleaner>(cleaner), id, userValues);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddCleaner([FromBody] CleanerRegisterRequest cleaner)
    {
        int id = _cleanersService.CreateCleaner(_mapper.Map<Cleaner>(cleaner));
        return Created($"{this.GetRequestFullPath()}/{id}", id);
    }

    [AuthorizeRoles(Role.Cleaner)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult DeleteCleaner(int id)
    {
        userValues = this.GetClaimsValue();
        _cleanersService.DeleteCleaner(id, userValues);
        return NoContent();
    }

    [AuthorizeRoles(Role.Cleaner)]
    [HttpGet("{id}/comments")]
    [ProducesResponseType(typeof(List<CommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<List<CommentResponse>> GetAllCommentsByCleaner (int id)
    {
        userValues = this.GetClaimsValue();
        var comments = _cleanersService.GetCommentsByCleaner(id, userValues);
        return Ok(_mapper.Map<List<CommentResponse>>(comments));
    }

    [AuthorizeRoles(Role.Client)]
    [HttpGet("{id}/orders")]
    [ProducesResponseType(typeof(List<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<List<OrderResponse>> GetAllOrdersByCleaner(int id)
    {
        userValues = this.GetClaimsValue();
        var orders = _cleanersService.GetOrdersByCleaner(id, userValues);
        return Ok(_mapper.Map<List<OrderResponse>>(orders));
    }
}

