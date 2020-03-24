using Microsoft.AspNetCore.Mvc;

namespace LedWallBackend.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IBimsInfo _bimsInfo;

        public TestController(IBimsInfo bimsInfo)
        {
            _bimsInfo = bimsInfo;
        }

        [HttpGet("IBims")]
        public IActionResult GetIBIms()
        {
            return Ok(_bimsInfo);
        }
    }
}