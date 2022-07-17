using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
public class ClientsController : ControllerBase
{
    private readonly IMapper _mapper;
    public List<string>? Identities;
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
    public ActionResult<ClientResponse> GetClient(int id)
    {
        Identities = this.GetClaimsValue();              
        var client = _clientsService.GetClient(id, Identities);
        return Ok(_mapper.Map<ClientResponse>(client));
    }

    [AuthorizeRoles]
    [HttpGet]
    [ProducesResponseType(typeof(List<ClientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<List<ClientResponse>> GetAllClients()
    {
        Identities = this.GetClaimsValue();
        var clients = _clientsService.GetAllClients(Identities);
        return Ok(_mapper.Map<List<ClientResponse>>(clients));
    }

    [AuthorizeRoles(Role.Client)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult UpdateClient([FromBody] ClientUpdateRequest client, int id)
    {
        Identities = this.GetClaimsValue();
        _clientsService.UpdateClient(_mapper.Map<Client>(client), id, Identities);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddClient([FromBody] ClientRegisterRequest client)
    {
        int id = _clientsService.CreateClient(_mapper.Map<Client>(client));
        return Created($"{this.GetRequestFullPath()}/{id}", id);
    }

    [AuthorizeRoles]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult DeleteClient(int id)
    {
        Identities = this.GetClaimsValue();
        _clientsService.DeleteClient(id, Identities);
        return NoContent();
    }

    [AuthorizeRoles(Role.Client)]
    [HttpGet("{id}/comments")]
    [ProducesResponseType(typeof(List<CommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult<List<CommentResponse>> GetAllCommentsByClient(int id)
    {
        Identities = this.GetClaimsValue();
        var comments = _clientsService.GetCommentsByClient(id, Identities);
        return Ok(_mapper.Map<List<OrderResponse>>(comments));
    }

    [AuthorizeRoles(Role.Client)]
    [HttpGet("{id}/orders")]
    [ProducesResponseType(typeof(List<CommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult<List<CommentResponse>> GetAllOrdersByClient(int id)
    {
        Identities = this.GetClaimsValue();
        var orders = _clientsService.GetOrdersByClient(id, Identities);
        return Ok(_mapper.Map<List<OrderResponse>>(orders));
    }
}