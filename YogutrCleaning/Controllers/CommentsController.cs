using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Repositories;
using YogurtCleaning.Enams;
using YogurtCleaning.Extensions;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]

public class CommentsController : Controller
{
    private readonly ILogger<CommentsController> _logger;
    private readonly ICommentsRepository _commentsRepository;

    public CommentsController(ILogger<CommentsController> logger, ICommentsRepository commentsRepository)
    {
        _logger = logger;
        _commentsRepository = commentsRepository;
    }

    [AuthorizeRoles]
    [HttpGet]
    [ProducesResponseType(typeof(List<CommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<List<CommentResponse>> GetAllComments()
    {
        var result = _commentsRepository.GetAllComments();
        return Ok(result);
    }

    [AuthorizeRoles(Role.Client)]
    [HttpPost("by-client")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddCommentByClient([FromBody] CommentRequest comment)
    {
        int commentId = new CommentResponse().Id;
        //var result = _commentsRepository.AddComment(comment);
        return Created($"{this.GetRequestFullPath()}/{commentId}", commentId);
    }

    [AuthorizeRoles(Role.Cleaner)]
    [HttpPost("by-cleaner")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddCommentByCleaner([FromBody] CommentRequest comment)
    {
        int commentId = new CommentResponse().Id;
        //var result = _commentsRepository.AddComment(comment);
        return Created($"{this.GetRequestFullPath()}/{commentId}", commentId);
    }

    [AuthorizeRoles]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult DeleteComment(int id)
    {
        _commentsRepository.DeleteComment(id);
        return NoContent();
    }
}
