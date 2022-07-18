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
public class ClientsController : ControllerBase
{
    private readonly IClientsRepository _clientsRepository;
    private readonly IMapper _mapper;
    private readonly IClientsService _clientsService;
    public ClientsController(IClientsRepository clientsRepository, IMapper mapper, IClientsService clientsService)
    {
        _clientsRepository = clientsRepository;
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
        var client = _clientsRepository.GetClient(id);
        if (client == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(_mapper.Map<ClientResponse>(client));
        }
    }

    [AuthorizeRoles]
    [HttpGet]
    [ProducesResponseType(typeof(List<ClientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<List<ClientResponse>> GetAllClients()
    {
        return Ok(_clientsRepository.GetAllClients());
    }

    [AuthorizeRoles(Role.Client)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult UpdateClient([FromBody] ClientUpdateRequest client, int id)
    {
        _clientsService.UpdateClient(_mapper.Map<Client>(client), id);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddClient([FromBody] ClientRegisterRequest client)
    {
        var id = _clientsRepository.CreateClient(_mapper.Map<Client>(client));
        return Created($"{this.GetRequestFullPath()}/{id}", id);
    }

    [AuthorizeRoles]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult DeleteClient(int id)
    {
        _clientsRepository.DeleteClient(id);
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
        return Ok(_clientsRepository.GetAllCommentsByClient(id));
    }
}