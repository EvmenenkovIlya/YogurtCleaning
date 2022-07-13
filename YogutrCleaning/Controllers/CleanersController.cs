using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.DataLayer.Repositories.Intarfaces;
using YogurtCleaning.Enams;
using YogurtCleaning.Extensions;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class CleanersController : ControllerBase
{
    private readonly ICleanersRepository _cleanersRepository;

    public CleanersController(ICleanersRepository cleanersRepository)
    {
        _cleanersRepository = cleanersRepository;
    }

    [AuthorizeRoles(Role.Cleaner)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CleanerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult<CleanerResponse> GetCleaner(int id)
    {
        var cleaner = _cleanersRepository.GetCleaner(id);
        if (cleaner == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(cleaner);
        }
    }

    [AuthorizeRoles]
    [HttpGet]
    [ProducesResponseType(typeof(List<CleanerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<List<CleanerResponse>> GetAllCleaners()
    {
        return Ok(_cleanersRepository.GetAllCleaners());
    }

    [AuthorizeRoles(Role.Cleaner)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult UpdateCleaner(int id, [FromBody] CleanerUpdateRequest model)
    {
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddCleaner([FromBody] CleanerRegisterRequest model)
    {
        var CleanerCreated = new CleanerResponse() { Id = 42 };
        return Created($"{this.GetRequestFullPath()}/{CleanerCreated.Id}", CleanerCreated.Id);
    }

    [AuthorizeRoles(Role.Cleaner)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult DeleteCleaner(int id)
    {
        _cleanersRepository.DeleteCleaner(id);
        return NoContent();
    }

    [AuthorizeRoles(Role.Cleaner)]
    [HttpGet("{id}/comments")]
    [ProducesResponseType(typeof(List<CommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<List<CommentResponse>> GetAllCommentsByCleaner (int id)
    {
        return Ok(_cleanersRepository.GetAllCommentsByCleaner(id)); ;
    }
}

