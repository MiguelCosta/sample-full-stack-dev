using AutoMapper;
using GigHub.Core;
using GigHub.Core.Dtos;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private IUnitOfWork _unitOfWork;

        public NotificationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<NotificationDto>> GetNewNotifications()
        {
            var user = User.Identity.GetUserId();
            var notifications = await _unitOfWork.Notifications.GetNotificationsUnread(user);

            return notifications.Select(Mapper.Map<NotificationDto>);
            // same result
            //return notifications.Select(Mapper.Map<Notification, NotificationDto>);
        }

        [HttpPost]
        public async Task<IHttpActionResult> MarkAsRead()
        {
            var userId = User.Identity.GetUserId();

            var notifications = await _unitOfWork.Notifications.GetUserNotificationsUnread(userId);

            notifications.ToList().ForEach(n => n.Read());

            await _unitOfWork.Complete();

            return Ok();
        }
    }
}
