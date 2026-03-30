using ApplicationLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationLayer.Repositories
{
    internal interface INotificationService
    {
        async Task DeliverAsync(UserSender user, Models.NotificationSender notification);
    }
}
