using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Business;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.Extensions;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("cleaning-objects")]
public class CleaningObjectsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICleaningObjectsService _cleaningObjectsService;
    private UserValues _userValues;

    public CleaningObjectsController(IMapper mapper, ICleaningObjectsService cleaningObjectsService)
    {
        _mapper = mapper;
        _cleaningObjectsService = cleaningObjectsService;
    }

    [AuthorizeRoles(Role.Cleaner,Role.Client)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CleaningObjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<CleaningObjectResponse>> GetCleaningObject(int id)
    {
        _userValues = this.GetClaimsValue();
        var cleaningObject = await _cleaningObjectsService.GetCleaningObject(id, _userValues);
        return Ok(_mapper.Map<CleaningObjectResponse>(cleaningObject));
    }

    [AuthorizeRoles(Role.Client)]
    [HttpGet]
    [ProducesResponseType(typeof(List<CleaningObjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<CleaningObjectResponse>>> GetAllCleaningObjectsByClientId(int clientId)
    {
        _userValues = this.GetClaimsValue();
        var cleaningObjects = await _cleaningObjectsService.GetAllCleaningObjectsByClientId(clientId, _userValues);
        return Ok(_mapper.Map<List<CleaningObjectResponse>>(cleaningObjects));
    }

    [AuthorizeRoles(Role.Client)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> UpdateCleaningObject([FromBody] CleaningObjectUpdateRequest model, int id)
    {
        _userValues = this.GetClaimsValue();
        await _cleaningObjectsService.UpdateCleaningObject(_mapper.Map<CleaningObject>(model), id, _userValues);
        return NoContent();
    }

    [AuthorizeRoles(Role.Client)]
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<int>> AddCleaningObject([FromBody] CleaningObjectRequest model)
    {
        _userValues = this.GetClaimsValue();
        int id = await _cleaningObjectsService.CreateCleaningObject(_mapper.Map<CleaningObject>(model), _userValues);
        return Created($"{this.GetRequestFullPath()}/{id}", id);
    }

    [AuthorizeRoles(Role.Client)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> DeleteCleaningObject(int id)
    {
        _userValues = this.GetClaimsValue();
        await _cleaningObjectsService.DeleteCleaningObject(id, _userValues);
        return NoContent();
    }
}
