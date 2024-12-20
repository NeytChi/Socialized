using Microsoft.AspNetCore.Mvc;
using WebAPI.response;

namespace WebAPI.Controllers
{
    [Route(Version.Current + "/[controller]/[action]/")]
    [ApiController]
    public class ControllerResponseBase : ControllerBase
    {
        public Serilog.ILogger Logger;

        [NonAction]
        public string GetAutorizationToken()
        {
            return HttpContext?.Request.Headers.Where(h => h.Key == "Authorization").Select(h => h.Value).FirstOrDefault();
        }
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