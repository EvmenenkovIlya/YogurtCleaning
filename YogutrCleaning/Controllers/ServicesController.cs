using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.Extensions;
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
    private readonly IServicesService _servicesService;
    private readonly IMapper _mapper;

    public ServicesController(ILogger<ServicesController> logger, IServicesRepository servicesRepository, IServicesService servicesService, IMapper mapper)
    {
        _logger = logger;
        _servicesRepository = servicesRepository;
        _servicesService = servicesService;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
    public ActionResult<ServiceResponse> GetService(int id)
    {
        var result = _mapper.Map<ServiceResponse>(_servicesRepository.GetService(id));
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ServiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
    public ActionResult<List<ServiceResponse>> GetAllServices()
    {
        var result = _mapper.Map<List<ServiceResponse>>(_servicesRepository.GetAllServices());
        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
    public ActionResult UpdateService([FromBody] ServiceRequest service, int id)
    {
        _servicesService.UpdateService(_mapper.Map<Service>(service), id);
        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(int), StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddService([FromBody] ServiceRequest service)
    {
        var result = _servicesService.AddService(_mapper.Map<Service>(service));
        return Created($"{this.GetRequestFullPath()}/{result}", result);
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