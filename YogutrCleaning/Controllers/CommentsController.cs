using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]

public class CommentsController : Controller
{
    private readonly ILogger<CommentsController> _logger;

    public CommentsController(ILogger<CommentsController> logger)
    {
        _logger = logger;
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CommentResponce), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<CommentResponce> GetComment(int id)
    {
        return Ok(new CommentResponce());
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpGet]
    [ProducesResponseType(typeof(List<CommentResponce>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult<List<CommentResponce>> GetAllComments()
    {
        return Ok(new List<CommentResponce>());
    }

    [AuthorizeRoles(Role.Client, Role.Cleaner)]
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddComment([FromBody] CommentRequest comment)
    {
        int commentId = new CommentResponce().Id;
        return Created($"{Request.Scheme}://{Request.Host.Value}{Request.Path.Value}/{commentId}", commentId);
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteComment(int id)
    {
        return Ok();
    }
}
