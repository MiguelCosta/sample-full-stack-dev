using AutoMapper;
using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                .Where(un => un.UserId == user)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist)
                .Include(n => n.Gig.Genre)
                .ToListAsync();

            return notifications.Select(Mapper.Map<NotificationDto>);
            // same result
            //return notifications.Select(Mapper.Map<Notification, NotificationDto>);
        }

    }
}
