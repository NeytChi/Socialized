using System;
using System.ComponentModel.DataAnnotations;

namespace UseCases.AutoPosts.Commands
{
    public class AutoPostCommand
    {
        [Required(ErrorMessage = "Account ID is required")]
        public long AccountId { get; set; }

        [Required(ErrorMessage = "Auto post type is required")]
        public bool AutoPostType { get; set; }

        [Required(ErrorMessage = "Auto delete is required")]
        public bool AutoDelete { get; set; }

        [Required(ErrorMessage = "Stopped status is required")]
        public bool Stopped { get; set; }

        [Required(ErrorMessage = "Execute at is required")]
        public DateTime ExecuteAt { get; set; }

        [Required(ErrorMessage = "Delete after is required")]
        public DateTime DeleteAfter { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        public long CategoryId { get; set; }

        [Required(ErrorMessage = "Time zone is required")]
        [Range(-12, 12, ErrorMessage = "Time zone must be between -12 and 12")]
        public int TimeZone { get; set; }

        public string? Location { get; set; }
        public string? Comment { get; set; }
        public string? Description { get; set; }
    }
}
