using Domain.AutoPosting;
using UseCases.Base;

namespace UseCases.AutoPosts.AutoPostFiles
{
    public interface IAutoPostFileSave
    {
        public bool CreateVideoFile(AutoPostFile post, FileDto file);
        public bool CreateImageFile(AutoPostFile post, FileDto file);
    }
}
