using System.ComponentModel.DataAnnotations;

namespace UseCases.Packages.Command
{
    public class GetClientTokenForPayCommand
    {
        [Required(ErrorMessage = "User token is required")]
        public string UserToken { get; set; } = string.Empty;
    }
}

