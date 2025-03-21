using Serilog;
using System.Web;
using Domain.Admins;
using Domain.AutoPosting;
using Domain.Users;
using UseCases.Exceptions;
using UseCases.Appeals.Commands;
using Domain.Appeals;

namespace UseCases.Appeals
{
    public class AppealManager : IAppealManager
    {
        private IAppealRepository AppealRepository;
        private IUserRepository UserRepository;
        private IAppealMessageRepository AppealMessageRepository;
        private ICategoryRepository CategoryRepository;
        private ILogger Logger;

        public AppealManager(ILogger logger,
            IAppealRepository appealRepository,
            IUserRepository userRepository,
            IAppealMessageRepository appealMessageRepository,
            ICategoryRepository categoryRepository)
        {
            Logger = logger;
            AppealRepository = appealRepository;
            UserRepository = userRepository;
            AppealMessageRepository = appealMessageRepository;
            CategoryRepository = categoryRepository;
        }
        public Appeal Create(CreateAppealCommand command)
        {
            var user = UserRepository.GetByUserTokenNotDeleted(command.UserToken);
            if (user == null)
            {
                throw new NotFoundException("���������� �� ��� ���������� �� ������.");
            }
            var appeal = new Appeal
            {
                UserId = user.Id,
                Subject = HttpUtility.UrlDecode(command.Subject),
                State = 1,
                CreatedAt = DateTime.UtcNow,
                LastActivity = DateTime.UtcNow
            };
            AppealRepository.Create(appeal);
            Logger.Information($"���� ��������� ���� �����, id={appeal.Id}.");
            return appeal;
        }
        public ICollection<Appeal> GetAppealsByUser(string userToken, int since, int count)
        {
            Logger.Information($"�������� ������ ������������, �={since} ��={count}.");
            return AppealRepository.GetAppealsBy(userToken, since, count);
        }
        public ICollection<Appeal> GetAppealsByAdmin(int since, int count)
        {
            Logger.Information($"�������� ������ ������, �={since} ��={count}.");
            return AppealRepository.GetAppealsBy(since, count);
        }
        public void UpdateAppealToClosed(long appealId)
        {
            var appeal = AppealRepository.GetBy(appealId);
            if (appeal == null)
            {
                throw new NotFoundException("��������� �� ���� ���������� �������� �� id.");
            }
            appeal.State = 4;
            AppealRepository.Update(appeal);
            Logger.Information($"��������� ���� �������, id={appeal.Id}.");
        }
        public void UpdateAppealToAnswered(long appealId)
        {
            var appeal = AppealRepository.GetBy(appealId);
            if (appeal == null)
            {
                throw new NotFoundException("��������� �� ���� ���������� �������� �� id.");
            }
            if (appeal.State == 1 || appeal.State == 2)
            {
                appeal.State = 3;
                AppealRepository.Update(appeal);
                Logger.Information($"�� ��������� �������. ��������� ���� ��������, id={appeal.Id}.");
            }
        }
        public void UpdateAppealToRead(int appealId)
        {
            var appeal = AppealRepository.GetBy(appealId);
            if (appeal == null)
            {
                throw new NotFoundException("��������� �� ���� ���������� �������� �� id.");
            }
            if (appeal.State == 1)
            {
                appeal.State = 2;
                AppealRepository.Update(appeal);
                Logger.Information($"��������� ���� ��������, �� ���������, id={appeal.Id}.");
            }
        }
    }
}