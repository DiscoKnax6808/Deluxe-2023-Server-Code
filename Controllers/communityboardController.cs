using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DeluxeNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class communityboardController : ControllerBase
    {
        [HttpGet("v2/current")]
        public async Task<IActionResult> getcb()
        {
            var j = JsonConvert.DeserializeObject(await System.IO.File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "Jsons", "communityboard.json")));

            return Ok(j);
        }
    }
}
