using Domain.Admins;
using UseCases.Appeals.Replies.Commands;

namespace UseCases.Appeals.Replies
{
    public interface IAppealMessageReplyManager
    {
        AppealMessageReply Create(CreateAppealMessageReplyCommand command);
        void Update(UpdateAppealMessageReplyCommand command);
        void Delete(DeleteAppealMessageReplyCommand command);
    }
}
