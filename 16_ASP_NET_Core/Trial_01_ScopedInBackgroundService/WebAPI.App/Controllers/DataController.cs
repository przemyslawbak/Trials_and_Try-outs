using Microsoft.AspNetCore.Mvc;

namespace Financial_IntradayScrapper.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DataController : ControllerBase
    {
        [HttpGet]
        [Route("/api/data/get-dupa")]
        public IActionResult GetDupa()
        {
            return Ok("dupa");
        }
    }
}
