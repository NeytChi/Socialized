using WebAPI.response;
using Microsoft.AspNetCore.Mvc;
using UseCases.Base;

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
        [NonAction]
        public ICollection<FileDto> Map(ICollection<IFormFile> formFiles)
        {
            var files = new List<FileDto>();
            foreach (var formFile in formFiles)
            {
                files.Add(new FileDto
                {
                    ContentType = formFile.ContentType,
                    ContentDisposition = formFile.ContentDisposition,
                    Length = formFile.Length,
                    Name = formFile.Name,
                    FileName = formFile.FileName,
                    Stream = formFile.OpenReadStream()
                });
            }
            return files;
        }
    }
}