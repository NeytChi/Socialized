using System.ComponentModel.DataAnnotations;

namespace UseCases.AutoPosts.AutoPostFiles.Commands
{
    public class UpdateAutoPostFileCommand : AutoPostFileCommand
    {
        [Required(ErrorMessage = "ID is required")]
        public long Id { get; set; }
    }
}