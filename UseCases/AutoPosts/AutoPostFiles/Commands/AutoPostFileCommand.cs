using System.ComponentModel.DataAnnotations;

namespace UseCases.AutoPosts.AutoPostFiles.Commands
{
    public class AutoPostFileCommand
    {
        [Range(-128, 127, ErrorMessage = "Order must be between -128 and 127")]
        public sbyte Order { get; set; }
    }
}
