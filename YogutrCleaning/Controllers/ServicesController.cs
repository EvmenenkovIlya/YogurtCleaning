using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.DataLayer.Repositories.Interfaces;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[AuthorizeRoles]
[Route("[controller]")]
public class ServicesController : ControllerBase
{
    private readonly ILogger<ServicesController> _logger;
    private readonly IServicesRepository _servicesRepository;

    public ServicesController(ILogger<ServicesController> logger, IServicesRepository servicesRepository)
    {
        _logger = logger;
        _servicesRepository = servicesRepository;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
    public ActionResult<ServiceResponse> GetService(int id)
    {
        var result = _servicesRepository.GetService(id);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ServiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
    public ActionResult<List<ServiceResponse>> GetAllServices()
    {
        var result = _servicesRepository.GetAllServices();
        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
    public ActionResult UpdateService([FromBody] ServiceRequest service, int id)
    {
        //var result = _servicesRepository.UpdateService(id);
        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(int), StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddService([FromBody] ServiceRequest service)
    {
        //var result = _servicesRepository.AddService(service);
        int userId = new ServiceResponse().Id;
        return Created($"{Request.Scheme}://{Request.Host.Value}{Request.Path.Value}/{userId}", userId);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
    public ActionResult DeleteService(int id)
    {
        _servicesRepository.DeleteService(id);
        return Ok();
    }
}