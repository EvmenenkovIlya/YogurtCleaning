﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.DataLayer.Repositories.Intarfaces;
using YogurtCleaning.Enams;
using YogurtCleaning.Extensions;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("cleaning-object")]
public class CleaningObjectController : ControllerBase
{
    private readonly ICleaningObjectsRepository _cleaningObjectsRepository;

    public CleaningObjectController(ICleaningObjectsRepository cleaningObjectsRepository)
    {
        _cleaningObjectsRepository = cleaningObjectsRepository;
    }

    [AuthorizeRoles(Role.Cleaner,Role.Client)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CleaningObjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<CleaningObjectResponse> GetCleaningObject(int id)
    {
        return Ok(new CleaningObjectResponse() { Id = id });
    }

    [AuthorizeRoles(Role.Client)]
    [HttpGet]
    [ProducesResponseType(typeof(List<CleaningObjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<List<CleaningObjectResponse>> GetAllCleaningObjectsByClientId(int clientId)
    {
        return Ok(new List<CleaningObjectResponse>());
    }
    [AuthorizeRoles(Role.Client)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status422UnprocessableEntity)]
    public ActionResult UpdateCleaningObject(int id, [FromBody] CleaningObjectUpdateRequest model)
    {
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
        var CleaningObject = new CleaningObjectResponse() { Id = 42 };
        return Created($"{this.GetRequestFullPath()}/{CleaningObject.Id}", CleaningObject.Id);
    }

    [AuthorizeRoles(Role.Client)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult DeleteCleaningObject(int id)
    {
        return NoContent();
    }
}
