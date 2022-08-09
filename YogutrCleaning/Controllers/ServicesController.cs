using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Business;
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
    private readonly IServicesService _servicesService;
    private readonly IMapper _mapper;

    public ServicesController(IServicesService servicesService, IMapper mapper)
    {
        _servicesService = servicesService;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ServiceResponse>> GetService(int id)
    {
        var result =  _mapper.Map<ServiceResponse>(await _servicesService.GetService(id));
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ServiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(int), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<ServiceResponse>>> GetAllServices()
    {
        var result = await _servicesService.GetAllServices();
        return Ok(_mapper.Map<List<ServiceResponse>>(result));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> UpdateService([FromBody] ServiceRequest service, int id)
    {
        await _servicesService.UpdateService(_mapper.Map<Service>(service), id);
        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<int>> AddService([FromBody] ServiceRequest service)
    {
        var result = await _servicesService.AddService(_mapper.Map<Service>(service));
        return Created($"{this.GetRequestFullPath()}/{result}", result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> DeleteService(int id)
    {
        UserValues userValues = this.GetClaimsValue();
       await _servicesService.DeleteService(id, userValues);
        return NoContent();
    }
}