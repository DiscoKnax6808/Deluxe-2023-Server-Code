using DeluxeNET.Data;
using DeluxeNET.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeluxeNET.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class accountController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly jwt _jwt;

        public accountController(AppDbContext db, jwt __jwt)
        {
            _db = db;
            _jwt = __jwt;
        }

        [HttpGet("bulk")]
        public async Task<IActionResult> BulkAccountDetails([FromQuery] List<long> id)
        {
            var accounts = await _db.Accounts
                .Where(x => id.Contains(x.accountId))
                .ToListAsync();

            return Ok(accounts);
        }

        [HttpGet("me")]
        public async Task<IActionResult> getMyAccountDetails()
        {
            foreach (var key in Request.Headers.Keys)
            {
                Console.WriteLine($"{key} = {Request.Headers[key]}");
            }

            Console.WriteLine(Request.Headers["Authorization"].ToString());

            var accountId = _jwt.VerifyToken(Request.Headers["Authorization"].ToString());

            if (accountId == null) return Unauthorized();

            var userdetails = await _db.Accounts
                .FirstOrDefaultAsync(x => x.accountId == accountId);

            return Ok(userdetails);
        }

        [HttpGet("{playerid}/bio")]
        public async Task<IActionResult> getPlayerBio([FromRoute] long playerid)
        {
            var player = await _db.Accounts
                .FirstOrDefaultAsync(x => x.accountId == playerid);

            

            return Ok(new
            {
                accountId = playerid,
                bio = player.bio ?? null
            });
        }

    }
}
