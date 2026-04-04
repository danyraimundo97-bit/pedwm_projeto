using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ApplicationLayer.Services
{
    public class SessionService : ISessionService
    {
        private User _user;
        private readonly IAppLogger _appLogger;
        public SessionService(IAppLogger appLogger)
        {
            _appLogger = appLogger;
            _user = GetSU();
        }

        private User GetSU()
        {
            _appLogger.LogInfo("Session is from SuperUser");
            UserBuilder userBuilder = new UserBuilder();
            return userBuilder
                .WithId(new Guid())
                .WithRole(UserRole.Admin)
                .WithName("Admin")
                .WithEmail("Admin@test.com")
                .Build();
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
