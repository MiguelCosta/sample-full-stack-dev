using GigHub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);

            if(gig.IsCanceled)
            {
                return NotFound();
            }

            gig.IsCanceled = true;

            var notification = new Notification
            {
                DateTime = DateTime.Now,
                Gig = gig,
                Type = NotificationType.GigCanceled
            };

            var attendees = await _context.Attendances
                .Where(a => a.GigId == gig.Id)
                .Select(a => a.Attendee).ToListAsync();

            foreach(var attende in attendees)
            {
                var userNotification = new UserNotification
                {
                    IsRead = false,
                    Notification = notification,
                    User = attende
                };
                _context.UserNotifications.Add(userNotification);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
