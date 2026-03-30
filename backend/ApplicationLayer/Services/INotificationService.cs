using ApplicationLayer.Models;
using DomainLayer.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationLayer.Services
{
    public interface INotificationService
    {
        Task DeliverAsync(Notification notification);
    }
}
