using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TittleAdmin.Model.DTO
{
    public class CustomNotification
    {
        public long id { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Time Start")]
        public string OnDate { get; set; }
        public string NextNotificationDate { get; set; }
        [Display(Name = "Content")]
        public string content { get; set; }
        [Display(Name = "Type")]
        public string type { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        public string data { get; set; }
        public NotificationTypes notificationType { get; set; }
        public NotificationStatus notificationStatus { get; set; }
    }
}
