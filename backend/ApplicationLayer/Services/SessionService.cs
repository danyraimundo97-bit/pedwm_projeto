using ApplicationLayer.Repositories;
using DomainLayer.Domain.Users;

namespace ApplicationLayer.Services
{
    public class SessionService : ISessionService
    {
        private User _user;
        private readonly IAppLogger _appLogger;

        public SessionService(IAppLogger appLogger, IUserRepository userRepository)
        {
            _appLogger = appLogger;
            _user = userRepository.GetByIdAsync(SuperUser.Id).GetAwaiter().GetResult()
                ?? throw new InvalidOperationException(
                    "Super user is missing from the database. Ensure migrations ran and startup seeding completed.");

            _appLogger.LogInfo($"Session bound to super user '{_user.Name}' (id {SuperUser.Id}).");
        }

        public void ChangeUserSession(User user)
        {
            if (user != null)
            {
                _user = user;
                _appLogger.LogInfo("Session Has Changed too");
            }
        }

        public User GetCurrentUser()
        {
            return _user;
        }
        
        public Guid GetCurrentUserID()
        {
            return _user.Id;
        }
    }
}
