using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TittleAdmin.Model.DTO
{
    public class CustomNotify
    {
        public List<long> ToIds { get; set; }
        public List<string> ToEmails { get; set; }
        public string To { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public Nullable<DateTime> TimeStart { get; set; }
        public string Subject { get; set; }
        [Required]
        [AllowHtml]
        public string Content { get; set; }
    }
}
