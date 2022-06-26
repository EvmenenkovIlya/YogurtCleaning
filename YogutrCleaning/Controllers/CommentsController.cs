using Microsoft.AspNetCore.Mvc;

namespace YogutrCleaning.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CommentsController : Controller
    {
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(ILogger<CommentsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public Comment GetComment(int id)
        {
            return new Comment();
        }

        [HttpGet]
        public List<Comment> GetAllComments()
        {
            return new List<Comment>();
        }

        [HttpPost("{summary}/{clientId}/{cleanerId}/{orderId}/{rating}")]
        public int AddComment(string summary, int clientId, int cleanerId, int orderId, int rating)
        {
            return new Comment().Id;
        }

        [HttpDelete("{id}")]
        public int DeleteComment(int id)
        {
            return new Comment().Id;
        }
    }
}
