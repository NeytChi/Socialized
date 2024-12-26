using Domain.Admins;
using UseCases.Appeals.Messages.Commands;

namespace UseCases.Appeals.Messages
{
    public interface IAppealMessageManager
    {
        AppealMessage Create(CreateAppealMessageCommand command);
        void Update(UpdateAppealMessageCommand command);
        void Delete(DeleteAppealMessageCommand command);
    }
}
