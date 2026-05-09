using DeluxeNET.Data;
using DeluxeNET.Jsons;
using DeluxeNET.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace DeluxeNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class avatarController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly jwt _jwt;

        public avatarController(AppDbContext db, jwt jwt)
        {
            _db = db;
            _jwt = jwt;
        }


        [HttpGet("v1/defaultunlocked")]
        public IActionResult getDefaultUnlockedAvatarITems()
        {
            return Ok(new List<object>());
        }

        [HttpGet("v1/defaultbaseavataritems")]
        public IActionResult getDefaultBaseItems()
        {
            return Ok(new List<object>());
        }

        [HttpGet("v4/items")]
        public IActionResult getUnlockedAvatarItems()
        {

            var clothingfile = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Jsons", "clothing.json"));

            var clothing = JsonConvert.DeserializeObject<List<avatarItem>>(clothingfile);

            return Ok(clothing);
        }

        [HttpGet("v2")]
        public async Task<IActionResult> getMyAvatar()
        {
            var accountId = _jwt.VerifyToken(Request.Headers["Authorization"]);

            if (accountId == null) return Unauthorized();

            var avatardetails = await _db.Avatars
                .FirstOrDefaultAsync(x => x.accountId == accountId);

            if (avatardetails == null)
            {
                _db.Avatars.Add(new avatar
                {
                    accountId = accountId
                });

                await _db.SaveChangesAsync();

                avatardetails = new avatar
                {
                    Id = 67,
                    accountId = accountId,
                    OutfitSelections = "",
                    FaceFeatures = "",
                    SkinColor = "",
                    HairColor = ""
                };
            }

            return Ok(avatardetails);
        }

        [HttpGet("v3/saved")]
        public IActionResult getsavedoutfits()
        {
            return Ok(new List<object>());
        }

        [HttpGet("v2/gifts")]
        public IActionResult getGifts()
        {
            return Ok(new List<object>());
        }

        [HttpPost("v2/set")]
        public IActionResult setAvatar()
        {
            return Ok();
        }
    }

    
}
