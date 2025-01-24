using AutoMapper;
using Core;
using Domain.Users;
using Serilog;
using UseCases.Exceptions;
using UseCases.Users.Commands;
using UseCases.Users.Response;

namespace UseCases.Users
{
    public class UserLoginManager : BaseManager, IUserLoginManager
    {
        private IMapper Mapper;
        private IUserRepository UserRepository;
        private ProfileCondition ProfileCondition = new ProfileCondition();

        public UserLoginManager(ILogger logger,
            IUserRepository userRepository,
            IMapper mapper) : base(logger) 
        {
            UserRepository = userRepository;
            Mapper = mapper;
        }
        public UserResponse Login(LoginUserCommand command)
        {
            Logger.Information($"Початок входу(логіну) користувача, email={command.Email}.");
            var user = UserRepository.GetByEmail(command.Email);
            if (user == null)
            {
                throw new NotFoundException("Сервер не визначив користувача по email для логіну.");
            }
            if (!ProfileCondition.VerifyHashedPassword(user.Password, command.Password))
            {
                throw new ValidationException("Пароль користувача не співпадає з паролем на сервері.");
            }
            user.LastLoginAt = DateTime.UtcNow;
            UserRepository.Update(user);
            Logger.Information($"Користувач був залогінен, id={user.Id}.");
            return Mapper.Map<UserResponse>(user);
        }
        public void LogOut(string userToken)
        {
            Logger.Information($"Початок виходу(logout) користувача.");
            var user = UserRepository.GetByUserTokenNotDeleted(userToken);

            if (user == null)
            {
                throw new NotFoundException("Сервер не визначив користувача по токен для активації аккаунту.");
            }
            user.TokenForUse = ProfileCondition.CreateHash(40);
            UserRepository.Update(user);
            Logger.Information($"Користувач вийшов з сервісу, id={user.Id}.");
        }
    }
}