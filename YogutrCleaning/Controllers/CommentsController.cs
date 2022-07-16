using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
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
[Route("[controller]")]

public class CommentsController : Controller
{
    private readonly ILogger<CommentsController> _logger;
    private readonly ICommentsRepository _commentsRepository;
    private readonly ICommentsService _commentsService;
    private readonly IMapper _mapper;

    public CommentsController(ILogger<CommentsController> logger, ICommentsRepository commentsRepository, ICommentsService commentsService, IMapper mapper)
    {
        _logger = logger;
        _commentsRepository = commentsRepository;
        _commentsService = commentsService;
        _mapper = mapper;
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
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = int.TryParse(userIdClaim, out var id) ? id : 0;

        var result = _commentsService.AddCommentByClient(_mapper.Map<Comment>(comment), id);
        return Created($"{this.GetRequestFullPath()}/{result}", result);
    }

    [AuthorizeRoles(Role.Cleaner)]
    [HttpPost("by-cleaner")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddCommentByCleaner([FromBody] CommentRequest comment)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = int.TryParse(userIdClaim, out var id) ? id : 0;

        var result = _commentsService.AddCommentByCleaner(_mapper.Map<Comment>(comment), id);
        return Created($"{this.GetRequestFullPath()}/{result}", result);
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
