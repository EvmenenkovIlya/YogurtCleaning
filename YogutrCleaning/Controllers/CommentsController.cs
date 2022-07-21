using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.Enams;
using YogurtCleaning.Extensions;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;
using YogurtCleaning.Business;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]

public class CommentsController : Controller
{
    private readonly ILogger<CommentsController> _logger;
    private readonly ICommentsService _commentsService;
    private readonly IMapper _mapper;
    public UserValues? userValues;

    public CommentsController(ILogger<CommentsController> logger, ICommentsService commentsService, IMapper mapper)
    {
        _logger = logger;
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
        var result = _commentsService.GetComments();
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
        var userId = this.GetClaimsValue().Id;

        var result = _commentsService.AddCommentByClient(_mapper.Map<Comment>(comment), userId);
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
        var userId = this.GetClaimsValue().Id;

        var result = _commentsService.AddCommentByCleaner(_mapper.Map<Comment>(comment), userId);
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
        _commentsService.DeleteComment(id);
        return NoContent();
    }
}
