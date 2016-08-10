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
        private ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsUnread(string userId)
        {
            return await _context.UserNotifications
                .Where(un => un.UserId == userId && un.IsRead == false)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist)
                .Include(n => n.Gig.Genre)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserNotification>> GetUserNotificationsUnread(string userId)
        {
            return await _context.UserNotifications
                .Where(un => un.UserId == userId && un.IsRead == false)
                .ToListAsync();
        }
    }
}
