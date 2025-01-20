using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace UseCases.AutoPosts.AutoPostFiles.Commands
{
    public class CreateAutoPostFileCommand : AutoPostFileCommand
    {
        [Required(ErrorMessage = "Form file is required")]
        public required IFormFile FormFile { get; set; }
    }
}
