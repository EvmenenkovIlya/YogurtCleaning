using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[AuthorizeRoles]
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
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
    public ActionResult<ServiceResponse> GetService(int id)
    {
        return Ok(new ServiceResponse());
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ServiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
    public ActionResult<List<ServiceResponse>> GetAllServices()
    {
        return Ok(new List<ServiceResponse>());
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
    public ActionResult UpdateService([FromBody] ServiceRequest service, int id)
    {
        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(int), StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddService([FromBody] ServiceRequest service)
    {
        int userId = new ServiceResponse().Id;
        return Created($"{Request.Scheme}://{Request.Host.Value}{Request.Path.Value}/{userId}", userId);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
    public ActionResult DeleteService(int id)
    {
        return Ok();
    }
}