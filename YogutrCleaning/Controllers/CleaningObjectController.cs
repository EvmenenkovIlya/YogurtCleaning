using Microsoft.AspNetCore.Mvc;

namespace YogurtCleaning.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CleaningObjectController
    {
        private readonly ILogger<CleanerController> _logger;
        public CleaningObjectController(ILogger<CleanerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public CleaningObject GetCleaningObject(int id)
        {
            return new CleaningObject();
        }

        [HttpGet]
        public List<CleaningObject> GetAllCleaningObjects()
        {
            return new List<CleaningObject>();
        }

        [HttpPut("{id}")]
        public void UpdateCleaningObject(int id, [FromBody] CleaningObject model)
        {
        }

        [HttpPost]
        public int AddCleaningObject([FromBody] CleaningObject model)
        {
            return new CleaningObject().Id;
        }

        [HttpDelete("{id}")]
        public int DeleteCleaningObject(int id)
        {
            return new CleaningObject().Id;
        }
    }
}
