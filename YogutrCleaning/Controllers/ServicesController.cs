using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class ServicesController : ControllerBase
{
    private readonly ILogger<ServicesController> _logger;

    public ServicesController(ILogger<ServicesController> logger)
    {
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ServiceResponce), StatusCodes.Status200OK)]
    public ActionResult<ServiceResponce> GetService(int id)
    {
        return Ok(new ServiceResponce());
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ServiceResponce>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult<List<ServiceResponce>> GetAllServices()
    {
        return Ok(new List<ServiceResponce>());
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult UpdateService([FromBody] ServiceRequest service, int id)
    {
        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddService([FromBody] ServiceRequest service)
    {
        int userId = new ServiceResponce().Id;
        return Created($"{Request.Scheme}://{Request.Host.Value}{Request.Path.Value}/{userId}", userId);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult DeleteService(int id)
    {
        return Ok();
    }
}