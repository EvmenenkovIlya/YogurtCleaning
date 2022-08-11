using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Business;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Extensions;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IMapper _mapper;
    public UserValues? userValues;
    private readonly IClientsService _clientsService;

    public ClientsController(IMapper mapper, IClientsService clientsService)
    {
        _mapper = mapper;
        _clientsService = clientsService;
    }  
    
    [AuthorizeRoles(Role.Client)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientResponse>> GetClient(int id)
    {
        userValues = this.GetClaimsValue();              
        var client = await _clientsService.GetClient(id, userValues);
        return Ok(_mapper.Map<ClientResponse>(client));
    }

    [AuthorizeRoles]
    [HttpGet]
    [ProducesResponseType(typeof(List<ClientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<ClientResponse>>> GetAllClients()
    {
        var clients = await _clientsService.GetAllClients();
        return Ok(_mapper.Map<List<ClientResponse>>(clients));
    }

    [AuthorizeRoles(Role.Client)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> UpdateClient([FromBody] ClientUpdateRequest client, int id)
    {
        userValues = this.GetClaimsValue();
        await _clientsService.UpdateClient(_mapper.Map<Client>(client), id, userValues);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<int>> AddClient([FromBody] ClientRegisterRequest client)
    {
        int id = await _clientsService.CreateClient(_mapper.Map<Client>(client));
        return Created($"{this.GetRequestFullPath()}/{id}", id);
    }

    [AuthorizeRoles]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> DeleteClient(int id)
    {
        userValues = this.GetClaimsValue();
        await _clientsService.DeleteClient(id, userValues);
        return NoContent();
    }

    [AuthorizeRoles(Role.Client)]
    [HttpGet("{id}/comments-by-client")]
    [ProducesResponseType(typeof(List<CommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<CommentResponse>>> GetAllCommentsByClient(int id)
    {
        userValues = this.GetClaimsValue();
        var comments = await _clientsService.GetCommentsByClient(id, userValues);
        return Ok(_mapper.Map<List<CommentResponse>>(comments));
    }

    [AuthorizeRoles(Role.Client)]
    [HttpGet("{id}/orders")]
    [ProducesResponseType(typeof(List<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<OrderResponse>>> GetAllOrdersByClient(int id)
    {
        userValues = this.GetClaimsValue();
        var orders = await _clientsService.GetOrdersByClient(id, userValues);
        return Ok(_mapper.Map<List<OrderResponse>>(orders));
    }

    [AuthorizeRoles(Role.Client)]
    [HttpGet("{id}/comments-about-client")]
    [ProducesResponseType(typeof(List<CommentAboutResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<CommentAboutResponse>>> GetCommentsAboutClient(int id)
    {
        userValues = this.GetClaimsValue();
        var comments = await _clientsService.GetCommentsAboutClient(id, userValues);
        return Ok(_mapper.Map<List<CommentAboutResponse>>(comments));
    }
}