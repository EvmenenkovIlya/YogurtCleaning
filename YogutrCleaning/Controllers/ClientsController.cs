using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.DataLayer.Repositories.Intarfaces;
using YogurtCleaning.Enams;
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

    public ClientsController(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }   

    [AuthorizeRoles(Role.Client)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult<ClientResponse> GetClient(int id)
    {
        return Ok(new ClientResponse() { Id = id });
    }

    [AuthorizeRoles]
    [HttpGet]
    [ProducesResponseType(typeof(List<ClientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<List<ClientResponse>> GetAllClients()
    {
        return Ok(new List<ClientResponse>());
    }

    [AuthorizeRoles(Role.Client)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult UpdateClient([FromBody] ClientUpdateRequest client)
    {
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddClient([FromBody] ClientRegisterRequest client)
    {       
        var clientCreated = new ClientResponse() { Id = 5 };
        return Created($"{this.GetRequestFullPath()}/{clientCreated.Id}", clientCreated.Id);
    }

    [AuthorizeRoles]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult DeleteClient(int id)
    {
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
        return Ok(new List<CommentResponse>()); ;
    }

    [AuthorizeRoles(Role.Client)]
    [HttpGet("{id}/comments/{commentId}")]
    [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult<CommentResponse> GetComment(int id, int commentId)
    {
        return Ok(new CommentResponse());
    }
}
