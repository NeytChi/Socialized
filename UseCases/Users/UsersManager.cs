using Core;
using Serilog;
using System.Web;
using Domain.Users;
using UseCases.Packages;
using UseCases.Users.Commands;
using UseCases.Exceptions;
using Domain.Packages;

namespace UseCases.Users
{
    public class UsersManager : BaseManager, IUsersManager
    {
        private IUserRepository UserRepository;
        
        public ProfileCondition ProfileCondition = new ProfileCondition();
        public IPackageManager PackageCondition;
        private IEmailMessanger EmailMessanger;

        public UsersManager(ILogger logger,
            IUserRepository userRepository,
            IEmailMessanger emailMessager,
            IPackageManager packageManager) : base(logger) 
        {
            UserRepository = userRepository;
            EmailMessanger = emailMessager;
            PackageCondition = packageManager;
        }
        public void Create(CreateUserCommand command)
        {
            var user = UserRepository.GetByEmail(command.Email);
            if (user != null && user.IsDeleted)
            {
                user.IsDeleted = false;
                user.TokenForUse = Guid.NewGuid().ToString();
                UserRepository.Update(user);
                Logger.Information($"��� ����������� ��������� �������, id={user.Id}.");
                return;   
            }
            if (user != null && !user.IsDeleted)
            {
                throw new NotFoundException("���������� � ����� email-������� ��� ����.");
            }
            user = new User
            {
                Email = command.Email,
                FirstName = HttpUtility.UrlDecode(command.FirstName),
                LastName = HttpUtility.UrlDecode(command.LastName),
                Password = ProfileCondition.HashPassword(command.Password),
                HashForActivate = ProfileCondition.CreateHash(100),
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,
                TokenForUse = ProfileCondition.CreateHash(40),
                profile = new Profile
                {
                    CountryName = HttpUtility.UrlDecode(command.CountryName),
                    TimeZone = command.TimeZone
                },
                access = new ServiceAccess()
            };            
            UserRepository.Create(user);
            PackageCondition.CreateDefaultServiceAccess(user.Id);
            EmailMessanger.SendConfirmEmail(user.Email, command.Culture, user.HashForActivate);
            Logger.Information($"����� ���������� ��� ���������, id={user.Id}.");
        }
        public void RegistrationEmail(string userEmail, string culture)
        {
            var user = UserRepository.GetByEmail(userEmail);
            if (user == null)
            {
                throw new NotFoundException("������ �� �������� ����������� �� email ��� ��������� ��������.");
            }
            EmailMessanger.SendConfirmEmail(user.Email, culture, user.HashForActivate);
            Logger.Information($"³�������� ���� �� ������������ ��������� �����������, id={user.Id}.");                
        }
        public void Activate(string hash)
        {
            var user = UserRepository.GetByHash(hash, false, false);
            if (user == null)
            {
                throw new NotFoundException("������ �� �������� ����������� �� ���� ��� ��������� ��������.");
            }
            user.Activate = true;
            UserRepository.Update(user);
            Logger.Information($"���������� ��� ����������� ������� ���� � �����, id={user.Id}.");
        }
        public void Delete(string userToken)
        {
            var user = UserRepository.GetByUserTokenNotDeleted(userToken);
            if (user == null)
            {
                throw new NotFoundException("������ �� �������� ����������� �� ���� ������ ��� ��������� ��������.");                
            }
            user.IsDeleted = true;
            user.TokenForUse = null;
            UserRepository.Update(user);
            Logger.Information($"���������� ��� ���������, id={user.Id}.");
        }
    }
}