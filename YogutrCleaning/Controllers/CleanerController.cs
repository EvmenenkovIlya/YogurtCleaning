using Microsoft.AspNetCore.Mvc;

namespace YogurtCleaning.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CleanerController : ControllerBase
    {
        private readonly ILogger<CleanerController> _logger;

        public CleanerController(ILogger<CleanerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public Cleaner GetCleaner(int id)
        {
            return new Cleaner();
        }

        [HttpGet]
        public List<Cleaner> GetAllCleaner()
        {
            return new List<Cleaner>();
        }

        [HttpPut("{id}")]
        public void UpdateCleaner(int id, [FromBody] Cleaner model)
        {
        }

        [HttpPost]
        public int AddCleaner([FromBody] Cleaner model)
        {
            return new Cleaner().Id;
        }

        [HttpDelete("{id}")]
        public int DeleteCleaner(int id)
        {
            return new Cleaner().Id;
        }
    }
}
