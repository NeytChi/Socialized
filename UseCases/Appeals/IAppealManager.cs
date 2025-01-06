using Domain.Admins;
using UseCases.Appeals.Commands;

namespace UseCases.Appeals
{
    public interface IAppealManager
    {
        Appeal Create(CreateAppealCommand command);
        ICollection<Appeal> GetAppealsByUser(string userToken, int since, int count);
        ICollection<Appeal> GetAppealsByAdmin(int since, int count);
        void UpdateAppealToClosed(long appealId);
        void UpdateAppealToAnswered(long appealId);
    }
}
