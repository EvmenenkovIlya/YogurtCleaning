using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [ProducesResponseType(typeof(BundleResponce), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<BundleResponce> GetBundle(int id)
    {
        return Ok(new BundleResponce());
    }
    [AuthorizeRoles(Role.Admin)]
    [HttpGet]
    [ProducesResponseType(typeof(List<BundleResponce>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult<List<BundleResponce>> GetAllBundles()
    {
        return Ok(new List<BundleResponce>());
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult UpdateBundle([FromBody] BundleRequest bundle, int id)
    {
        return NoContent();
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddBundle([FromBody] BundleRequest bundle)
    {
        int bundleId = new BundleResponce().Id;
        return Created($"{Request.Scheme}://{Request.Host.Value}{Request.Path.Value}/{bundleId}", bundleId);
    }

    [AuthorizeRoles(Role.Admin)]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteBundle(int id)
    {
        return Ok();
    }
}