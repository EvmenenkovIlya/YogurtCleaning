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
    public ClientResponse GetClient(int id)
    {
        return new ClientResponse();
    }

    [AuthorizeRoles(Role.Admin, Role.Client)]
    [HttpGet]
    public List<ClientResponse> GetAllClients()
    {
        return new List<ClientResponse>();
    }

    [AuthorizeRoles(Role.Client)]
    [HttpPut("{id}")]
    public ClientResponse UpdateClient([FromBody] ClientUpdateRequest client, int id)
    {
        return new ClientResponse();
    }

    [HttpPost()]
    public int AddClient([FromBody] ClientRegisterRequest client)
    {
        return new Client().Id;
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpDelete("{id}")]
    public void DeleteClient(int id)
    {
       
    }
}
