using System.ComponentModel.DataAnnotations;

namespace UseCases.AutoPosts.AutoPostFiles.Commands
{
    public class DeleteAutoPostFileCommand
    {
        [Required(ErrorMessage = "Auto post ID is required")]
        public long AutoPostId { get; set; }
    }
}
