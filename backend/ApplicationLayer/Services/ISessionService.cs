using DomainLayer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationLayer.Services
{
    public interface ISessionService
    {
        void ChangeUserSession(User user);
        User GetCurrentUser();

        Guid GetCurrentUserID();
    }
}
