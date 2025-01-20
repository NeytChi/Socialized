using System.ComponentModel.DataAnnotations;

namespace UseCases.Admins.Commands
{
    public class DeleteAdminCommand
    {
        [Required(ErrorMessage = "Admin ID is required")]
        public long AdminId { get; set; }
    }
}

