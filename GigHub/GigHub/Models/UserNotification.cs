using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Models
{
    public class UserNotification
    {
        protected UserNotification()
        {
            // é necessário para a EntityFramework
            // mas assim não aparece disponível
            // para o developer utilizar no código
        }

        public UserNotification(ApplicationUser user, Notification notification)
        {
            if(user == null)
                throw new ArgumentNullException("user");

            if(notification == null)
                throw new ArgumentNullException("notification");

            User = user;
            Notification = notification;
            IsRead = false;
        }

        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int NotificationId { get; set; }

        public bool IsRead { get; private set; }

        public ApplicationUser User { get; private set; }

        public Notification Notification { get; private set; }

        public void Read()
        {
            IsRead = true;
        }
    }
}
