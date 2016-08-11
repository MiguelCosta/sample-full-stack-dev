using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GigHub.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private IApplicationDbContext _context;

        public NotificationRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsUnreadAsync(string userId)
        {
            return await _context.UserNotifications
                .Where(un => un.UserId == userId && un.IsRead == false)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserNotification>> GetUserNotificationsUnreadAsync(string userId)
        {
            return await _context.UserNotifications
                .Where(un => un.UserId == userId && un.IsRead == false)
                .ToListAsync();
        }
    }
}
