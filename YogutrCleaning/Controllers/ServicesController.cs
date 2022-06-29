using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
//[Authorize]
[Route("[controller]")]
public class ServicesController : ControllerBase
{
    private readonly ILogger<ServicesController> _logger;

    public ServicesController(ILogger<ServicesController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{id}")]
    public ServiceResponce GetService(int id)
    {
        return new ServiceResponce();
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpGet]
    public List<ServiceResponce> GetAllServices()
    {
        return new List<ServiceResponce>();
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpPut("{id}")]
    public void UpdateService([FromBody] ServiceRequest service, int id)
    {           
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpPost]
    public int AddService([FromBody] ServiceRequest service)
    {
        return new ServiceResponce().Id;
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpDelete("{id}")]
    public void DeleteService(int id)
    {
    }
}