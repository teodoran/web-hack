namespace Notes.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
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