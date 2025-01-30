/*using WebAPI.Responses;
using Domain.AutoPosting;
using UseCases.AutoPosts;
using UseCases.AutoPosts.Commands;
using UseCases.AutoPosts.AutoPostFiles.Commands;
using UseCases.AutoPosts.AutoPostFiles;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class AutoPostController : ControllerResponseBase
    {
        private IAutoPostManager AutoPostManager;
        private IAutoPostFileManager AutoPostFileManager;
        
        public AutoPostController(IAutoPostManager autoPostManager, 
            IAutoPostFileManager autoPostFileManager)
        {
            AutoPostManager = autoPostManager;
            AutoPostFileManager = autoPostFileManager;
        }
        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public ActionResult<dynamic> Create([FromForm] ICollection<IFormFile> files,[FromForm] CreateAutoPostCommand command)
        {
            sbyte index = 0;
            foreach (var file in files) 
            {
                command.Files.Add(new CreateAutoPostFileCommand
                {
                    FormFile = file,
                    Order = index++
                });
            }

            AutoPostManager.Create(command);

            return new SuccessResponse(true);
        }
        [HttpGet]
        public ActionResult<dynamic> Get([FromBody] string userToken,
            [FromQuery] long accountId, 
            [FromQuery] DateTime from, 
            [FromQuery] DateTime to, 
            [FromQuery] int since = 1, 
            [FromQuery] int count = 10)
        {
            var command = new GetAutoPostsCommand
            {
                UserToken = userToken, AccountId = accountId,
                From = from, To = to,
                Since = since, Count = count
            };

            AutoPostManager.Get(command);

            return new SuccessResponse(true);
        }
        [HttpPut]
        public ActionResult<dynamic> Update(UpdateAutoPostCommand command)
        {
            AutoPostManager.Update(command);

            return new SuccessResponse(true);
        }
        [HttpDelete]
        public ActionResult<dynamic> Delete(DeleteAutoPostCommand command)
        {
            AutoPostManager.Delete(command);

            return new SuccessResponse(true);
        }
        [HttpDelete]
        [ActionName("DeleteFile")]
        public ActionResult<dynamic> DeleteFile(DeleteAutoPostFileCommand command)
        {
            AutoPostFileManager.Delete(command);

            return new SuccessResponse(true);
        }
    }
}*/