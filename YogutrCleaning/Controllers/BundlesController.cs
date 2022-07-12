using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Enams;
using YogurtCleaning.Extensions;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class BundleController : ControllerBase
{
    private readonly ILogger<BundleController> _logger;

    public BundleController(ILogger<BundleController> logger)
    {
        _logger = logger;
    }
    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BundleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult<BundleResponse> GetBundle(int id)
    {
        return Ok(new BundleResponse());
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpGet]
    [ProducesResponseType(typeof(List<BundleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public ActionResult<List<BundleResponse>> GetAllBundles()
    {
        return Ok(new List<BundleResponse>());
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status422UnprocessableEntity)]
    public ActionResult UpdateBundle([FromBody] BundleRequest bundle, int id)
    {
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
        int bundleId = new BundleResponse().Id;
        return Created($"{this.GetRequestFullPath()}/{bundleId}", bundleId);
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    public ActionResult DeleteBundle(int id)
    {
        return Ok();
    }
}