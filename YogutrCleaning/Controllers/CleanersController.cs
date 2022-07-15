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
[Route("[controller]")]
public class CleanersController : ControllerBase
{
    private readonly ICleanersRepository _cleanersRepository;
    private readonly IMapper _mapper;
    private readonly ICleanersService _cleanersService;
    public CleanersController(ICleanersRepository cleanersRepository, IMapper mapper, ICleanersService cleanersService)
    {
        _cleanersRepository = cleanersRepository;
        _mapper = mapper;
        _cleanersService = cleanersService;
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
        _cleanersService.UpdateCleaner(_mapper.Map<Cleaner>(model), id);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddCleaner([FromBody] CleanerRegisterRequest model)
    {
        int id = _cleanersRepository.CreateCleaner(_mapper.Map<Cleaner>(model));
        return Created($"{this.GetRequestFullPath()}/{id}", id);
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

