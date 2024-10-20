using Microsoft.AspNetCore.Mvc;

namespace FeedbackSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        [Route("/error")]
        public IActionResult HandleError()
        {
            return Problem("An unexpected error occurred. Please try again later.");
        }
    }
}
