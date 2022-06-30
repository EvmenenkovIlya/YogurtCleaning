using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class ClientsController : ControllerBase
{
    private readonly ILogger<ClientsController> _logger;

    public ClientsController(ILogger<ClientsController> logger)
    {
        _logger = logger;
    }

    [AuthorizeRoles(Role.Client)]
    [HttpGet("{id}")]
    public ActionResult GetClient(int id)
    {
        //return
        var client = new ClientResponse() { Id = id};
        return Created($"Clients/{client.Id}", client.Id);
    }

    [AuthorizeRoles]
    [HttpGet]
    public List<ClientResponse> GetAllClients()
    {
        return new List<ClientResponse>();
    }

    [AuthorizeRoles(Role.Client)]
    [HttpPut("{id}")]
    public ActionResult UpdateClient([FromBody] ClientUpdateRequest client, int id)
    {
        return NoContent();
    }
    [AllowAnonymous]
    [HttpPost]
    public ActionResult AddClient([FromBody] ClientRegisterRequest client)
    {       
        var clientCreated = new ClientResponse() { Id = 5 };
        return Created($"{Request.Scheme}://{Request.Host.Value}{Request.Path.Value}/{clientCreated.Id}", clientCreated.Id);
    }
    [AuthorizeRoles]
    [HttpDelete("{id}")]
    public void DeleteClient(int id)
    {
       
    }
    [AuthorizeRoles(Role.Client)]
    [HttpGet("{id}/comments")]
    public List<Comment> GetAllCommentsByClient(int id)
    {
        return new();
    }
    [AuthorizeRoles(Role.Client)]
    [HttpGet("{id}/comments/{commentId}")]
    public Comment GetComment(int id, int commentId)
    {
        return new();
    }
}
