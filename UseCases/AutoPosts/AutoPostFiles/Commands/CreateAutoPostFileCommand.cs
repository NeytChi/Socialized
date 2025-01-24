using System.ComponentModel.DataAnnotations;
using UseCases.Base;

namespace UseCases.AutoPosts.AutoPostFiles.Commands
{
    public class CreateAutoPostFileCommand : AutoPostFileCommand
    {
        [Required(ErrorMessage = "Form file is required")]
        public required FileDto FormFile { get; set; }
    }
}
