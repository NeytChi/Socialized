using Microsoft.AspNetCore.Http;

namespace UseCases.AutoPosts.AutoPostFiles.Commands
{
    public class CreateAutoPostFileCommand : AutoPostFileCommand
    {
        public required IFormFile FormFile { get; set; }
    }
}
