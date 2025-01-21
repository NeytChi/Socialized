using WebAPI.response;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route(Version.Current + "/[controller]/[action]/")]
    [ApiController]
    public class ControllerResponseBase : ControllerBase
    {
        [NonAction]
        public string GetCulture()
        {
            return Request.Headers["Accept-Language"].FirstOrDefault() ?? "en_US";
        }
        [NonAction]
        public long GetAdminIdByToken()
        {
            return long.Parse(HttpContext.User.Claims.First().Value);
        }
        [NonAction]
        public ObjectResult StatusCode500(string message)
        {
            return StatusCode(500, new AnswerResponse(false, message));
        }
    }
}