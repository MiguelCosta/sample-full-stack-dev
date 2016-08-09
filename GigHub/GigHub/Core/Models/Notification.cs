using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Core.Models
{
    public class Notification
    {
        protected Notification()
        {

        }

        private Notification(Gig gig, NotificationType type)
        {
            if(gig == null)
                throw new ArgumentNullException("gig");

            DateTime = DateTime.Now;
            Gig = gig;
            Type = type;
        }

        public int Id { get; private set; }

        public DateTime DateTime { get; private set; }

        public NotificationType Type { get; private set; }

        public DateTime? OriginalDateTime { get; private set; }

        public string OriginalVenue { get; private set; }

        [Required]
        public Gig Gig { get; private set; }


        public static Notification GigCreated(Gig gig)
        {
            return new Notification(gig, NotificationType.GigCreated);

        }
        public static Notification GigUpdated(Gig newGig, DateTime originalDateTime, string originalVenue)
        {
            var notification = new Notification(newGig, NotificationType.GigUpdated);
            notification.OriginalDateTime = originalDateTime;
            notification.OriginalVenue = originalVenue;

            return notification;
        }

        public static Notification GigCanceled(Gig gig)
        {
            var notification = new Notification(gig, NotificationType.GigCanceled);
            return notification;
        }
    }
}
