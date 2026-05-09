using DeluxeNET.Models.ServerConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeluxeNET.Controllers
{
    [Route("/")]
    [ApiController]
    public class NameServerController : ControllerBase
    {
        private string BaseURL = "https://localhost";

        [HttpGet]
        public ActionResult<NameServerModel> NameServer()
        {
            return Ok(new NameServerModel
            {
                Accounts = BaseURL,
                API = BaseURL,
                Auth = BaseURL,
                BugReporting = BaseURL,
                Cards = BaseURL,
                CDN = BaseURL+"/cdnserver",
                Chat = BaseURL,
                Clubs = BaseURL,
                CMS = BaseURL,
                Commerce = BaseURL,
                Data = BaseURL,
                DataCollection = BaseURL,
                Discovery = BaseURL,
                Econ = BaseURL,
                GameLogs = BaseURL,
                Geo = BaseURL,
                Images = BaseURL+"/imageserver",
                Leaderboard = BaseURL,
                Link = BaseURL,
                Lists = BaseURL,
                Matchmaking = BaseURL,
                Moderation = BaseURL,
                Notifications = BaseURL,
                PlatformNotifications = BaseURL,
                PlayerSettings = BaseURL,
                RoomComments = BaseURL,
                Rooms = BaseURL+"/roomserver",
                Storage = BaseURL+"/cdnserver",
                Strings = BaseURL,
                StringsCDN = BaseURL+"/cdnserver",
                Studio = BaseURL,
                Thorn = BaseURL,
                Videos = BaseURL,
                WWW = BaseURL
            });
        }
    }
}
