using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("meta/healthcheck")]
    [ApiController]
    public class HealthcheckController: Controller
    {
        [HttpGet]
        public IActionResult Healthcheck()
        {
            return Ok("I'm healthy");
        }
    }
}
