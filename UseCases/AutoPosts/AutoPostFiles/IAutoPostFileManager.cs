using Domain.AutoPosting;
using UseCases.AutoPosts.AutoPostFiles.Commands;

namespace UseCases.AutoPosts.AutoPostFiles
{
    public interface IAutoPostFileManager
    {
        ICollection<AutoPostFile> Create(ICollection<CreateAutoPostFileCommand> files, sbyte startOrder);
        void Update(ICollection<UpdateAutoPostFileCommand> commandFiles, ICollection<AutoPostFile> autoPost);
        void Delete(DeleteAutoPostFileCommand command);
    }
}
