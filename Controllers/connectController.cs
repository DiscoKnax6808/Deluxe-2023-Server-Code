using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using DeluxeNET.Security;

namespace DeluxeNET.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class connectController : ControllerBase
    {

        private string steamkey = "Change to a valid steam id!";
        private readonly jwt _jwt;

        public connectController(jwt __jwt)
        {
            _jwt = __jwt;
        }

        [HttpPost("token")]
        public async Task<IActionResult> genTokenByAuth()
        {
            Console.WriteLine("=== Incoming Request ===");

            foreach (var key in Request.Form.Keys)
            {
                var value = Request.Form[key];
                Console.WriteLine($"{key} = {value}");
            }

            var platAuth = JsonConvert.DeserializeObject<platform_auth>(Request.Form["platform_auth"]);

            if (platAuth == null)
            {
                Console.WriteLine("platform_auth is null or failed to deserialize");
                return Unauthorized();
            }

            if (platAuth.AppId != "471710")
            {
                Console.WriteLine($"Invalid AppId: {platAuth.AppId}");
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(platAuth.Ticket))
            {
                Console.WriteLine("Ticket is null or empty");
                return Unauthorized();
            }

            var url = $"https://api.steampowered.com/ISteamUserAuth/AuthenticateUserTicket/v1/" +
                      $"?key={steamkey}&appid={platAuth.AppId}&ticket={platAuth.Ticket}";

            Console.WriteLine("Sending request to Steam...");

            using var client = new HttpClient();

            var response = await client.GetStringAsync(url);
            Console.WriteLine($"Steam response: {response}");

            var json = JsonDocument.Parse(response);
            var root = json.RootElement;

            if (!root.TryGetProperty("response", out var responseObj))
            {
                Console.WriteLine("Missing 'response' in Steam reply");
                return Unauthorized();
            }

            if (!responseObj.TryGetProperty("params", out var paramsObj))
            {
                Console.WriteLine("Missing 'params' in Steam reply");
                return Unauthorized();
            }

            if (!paramsObj.TryGetProperty("steamid", out var steamdIdElement))
            {
                Console.WriteLine("Missing 'steamid' in Steam reply");
                return Unauthorized();
            }

            var steamid = steamdIdElement.GetString();
            Console.WriteLine($"SteamID from Steam: {steamid}");

            var clientPlatformId = Request.Form["platform_id"].ToString();

            if (clientPlatformId != steamid)
            {
                Console.WriteLine($"platform_id mismatch. Client: {clientPlatformId}, Steam: {steamid}");
                return Unauthorized();
            }

            var accountId = Request.Form["account_id"].ToString();

            if (string.IsNullOrEmpty(accountId))
            {
                Console.WriteLine("account_id is missing");
                return Unauthorized();
            }

            var token = _jwt.GenerateToken(accountId);

            Console.WriteLine($"JWT generated for account_id: {accountId}");

            var resp = new
            {
                access_token = token,
                error_description = "",
                error = "",
                refresh = token,
                refresh_token = token,
                key = ""
            };

            Console.WriteLine(JsonConvert.SerializeObject(resp));

            return Ok(resp);
        }





        public class platform_auth
        {
            public required string Ticket { get; set; }
            public required string AppId { get; set; }
        }
    }
}
