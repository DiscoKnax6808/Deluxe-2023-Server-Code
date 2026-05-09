using DeluxeNET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DeluxeNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class playerReputationController : ControllerBase
    {
        private readonly AppDbContext _db;
        public playerReputationController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("v2/bulk")]
        public async Task<IActionResult> getRepBulk([FromQuery] List<long> id)
        {
            var reps = await _db.Reputations
                .Where(x => id.Contains(x.Id))
                .ToListAsync();

            return Ok(reps);
        }
    }
}
