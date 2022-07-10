using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YogurtCleaning.Enams;
using YogurtCleaning.Extensions;
using YogurtCleaning.Infrastructure;
using YogurtCleaning.Models;

namespace YogurtCleaning.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CleaningObjectController : ControllerBase
    {
        private readonly ILogger<CleanerController> _logger;
        public CleaningObjectController(ILogger<CleanerController> logger)
        {
            _logger = logger;
        }

        [AuthorizeRoles(Role.Cleaner,Role.Client)]
        [HttpGet("/Cleaning-Object/{id}")]
        [ProducesResponseType(typeof(CleaningObjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public ActionResult<CleaningObjectResponse> GetCleaningObject(int id)
        {
            return Ok(new CleaningObjectResponse() { Id = id });
        }

        [AuthorizeRoles(Role.Client)]
        [HttpGet("/Cleaning-Object")]
        [ProducesResponseType(typeof(List<CleaningObjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public ActionResult<List<CleaningObjectResponse>> GetAllCleaningObjectsByClientId(int clientId)
        {
            return Ok(new List<CleaningObjectResponse>());
        }
        [AuthorizeRoles(Role.Client)]
        [HttpPut("/Cleaning-Object/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status422UnprocessableEntity)]
        public ActionResult UpdateCleaningObject(int id, [FromBody] CleaningObjectUpdateRequest model)
        {
            return NoContent();
        }

        [AuthorizeRoles(Role.Client)]
        [HttpPost("/Cleaning-Object")]
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
        [HttpDelete("/Cleaning-Object/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public ActionResult DeleteCleaningObject(int id)
        {
            return NoContent();
        }
    }
}
