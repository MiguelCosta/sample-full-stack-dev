using AutoMapper;
using GigHub.Core.Dtos;
using GigHub.Persistence;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private ApplicationDbContext _context;

        public NotificationsController()
        {
            _context = new ApplicationDbContext();
        }

        public async Task<IEnumerable<NotificationDto>> GetNewNotifications()
        {
            var user = User.Identity.GetUserId();
            var notifications = await _context.UserNotifications
                .Where(un => un.UserId == user && un.IsRead == false)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist)
                .Include(n => n.Gig.Genre)
                .ToListAsync();

            return notifications.Select(Mapper.Map<NotificationDto>);
            // same result
            //return notifications.Select(Mapper.Map<Notification, NotificationDto>);
        }

        [HttpPost]
        public async Task<IHttpActionResult> MarkAsRead()
        {
            var userId = User.Identity.GetUserId();

            var notifications = await _context.UserNotifications
                .Where(un => un.UserId == userId && un.IsRead == false)
                .ToListAsync();

            notifications.ForEach(n => n.Read());

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
