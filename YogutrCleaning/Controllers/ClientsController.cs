using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [Authorize(Roles = nameof(Role.Client))]
    [HttpGet("{id}")]
    public ClientResponse GetClient(int id)
    {
        return new ClientResponse();
    }

    [Authorize(Roles = nameof(Role.Admin)+","+nameof(Role.Client))]
    [HttpGet]
    public List<ClientResponse> GetAllClients()
    {
        return new List<ClientResponse>();
    }

    [Authorize(Roles = nameof(Role.Client))]
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

    [Authorize(Roles = nameof(Role.Admin))]
    [HttpDelete("{id}")]
    public void DeleteClient(int id)
    {
       
    }
}
//public class AuthorizeByRoleAttribute : AuthorizeAttribute
//{
//    public AuthorizeAttribute(params Role[] roles)
//    {
//        for(int i = 0; i < roles.Length-1; i++)
//        {
//            Roles += nameof(roles[i]);
//        }

//    }
//}