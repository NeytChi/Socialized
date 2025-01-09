using Domain.AutoPosting;
using Microsoft.AspNetCore.Http;

namespace UseCases.AutoPosts.AutoPostFiles
{
    public interface IAutoPostFileSave
    {
        public bool CreateVideoFile(AutoPostFile post, IFormFile file);
        public bool CreateImageFile(AutoPostFile post, IFormFile file);
    }
}
