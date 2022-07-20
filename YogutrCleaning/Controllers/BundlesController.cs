using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Business.Services;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Extensions;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class BundlesController : ControllerBase
{
    private readonly ILogger<BundlesController> _logger;
    private readonly IBundlesService _bundlesService;
    private readonly IMapper _mapper;

    public BundlesController(ILogger<BundlesController> logger, IBundlesService bundlesService, IMapper mapper)
    {
        _logger = logger;
        _bundlesService = bundlesService;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BundleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult<BundleResponse> GetBundle(int id)
    {
        var result = _mapper.Map<BundleResponse>(_bundlesService.GetBundle(id));
        return Ok(result);
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpGet]
    [ProducesResponseType(typeof(List<BundleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<List<BundleResponse>> GetAllBundles()
    {
        var result = _mapper.Map<List<BundleResponse>>(_bundlesService.GetAllBundles());
        return Ok(result);
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status422UnprocessableEntity)]
    public ActionResult UpdateBundle([FromBody] BundleRequest bundle, int id)
    {
        _bundlesService.UpdateBundle(_mapper.Map<Bundle>(bundle), id);
        return NoContent();
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddBundle([FromBody] BundleRequest bundle)
    {
        var result = _bundlesService.AddBundle(_mapper.Map<Bundle>(bundle));
        return Created($"{this.GetRequestFullPath()}/{result}", result);
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult DeleteBundle(int id)
    {
        _bundlesService.DeleteBundle(id);
        return NoContent();
    }
}