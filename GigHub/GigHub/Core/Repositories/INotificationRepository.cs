﻿using GigHub.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GigHub.Core.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetNotificationsUnreadAsync(string userId);

        Task<IEnumerable<UserNotification>> GetUserNotificationsUnreadAsync(string userId);
    }
}
