using Microsoft.AspNetCore.Mvc;

namespace Notes.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        /// <summary>
        /// Returns PONG.
        /// </summary>
        [HttpGet]
        public string Get() => "PONG";
    }
}
