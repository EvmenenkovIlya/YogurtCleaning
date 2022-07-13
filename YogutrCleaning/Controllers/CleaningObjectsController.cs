using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.Enams;
using YogurtCleaning.Extensions;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("cleaning-objects")]
public class CleaningObjectsController : ControllerBase
{
    private readonly ICleaningObjectsRepository _cleaningObjectsRepository;
    private readonly IMapper _mapper;
    private readonly ICleaningObjectsService _cleaningObjectsService;

    public CleaningObjectsController(ICleaningObjectsRepository cleaningObjectsRepository, IMapper mapper, ICleaningObjectsService cleaningObjectsService)
    {
        _cleaningObjectsRepository = cleaningObjectsRepository;
        _mapper = mapper;
        _cleaningObjectsService = cleaningObjectsService;
    }

    [AuthorizeRoles(Role.Cleaner,Role.Client)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CleaningObjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<CleaningObjectResponse> GetCleaningObject(int id)
    {
        var cleaningObject = _cleaningObjectsRepository.GetCleaningObject(id);
        if (cleaningObject == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(cleaningObject);
        }
    }

    [AuthorizeRoles(Role.Client)]
    [HttpGet]
    [ProducesResponseType(typeof(List<CleaningObjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<List<CleaningObjectResponse>> GetAllCleaningObjectsByClientId(int clientId)
    {
        return Ok(_cleaningObjectsRepository.GetAllCleaningObjects());
    }

    [AuthorizeRoles(Role.Client)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status422UnprocessableEntity)]
    public ActionResult UpdateCleaningObject(int id, [FromBody] CleaningObjectUpdateRequest model)
    {
        _cleaningObjectsService.UpdateCleaningObject(_mapper.Map<CleaningObject>(model), id);
        return NoContent();
    }

    [AuthorizeRoles(Role.Client)]
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddCleaningObject([FromBody] CleaningObjectRequest model)
    {
        int id = _cleaningObjectsRepository.CreateCleaningObject(_mapper.Map<CleaningObject>(model));
        return Created($"{this.GetRequestFullPath()}/{id}", id);
    }

    [AuthorizeRoles(Role.Client)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult DeleteCleaningObject(int id)
    {
        _cleaningObjectsRepository.DeleteCleaningObject(id);
        return NoContent();
    }
}

