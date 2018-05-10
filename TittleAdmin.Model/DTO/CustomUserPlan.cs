using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TittleAdmin.Model.DTO
{
    public class CustomUserPlan
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Plan { get; set; }
        public string PromoCode { get; set; }
        public string StartDate { get; set; }
    }
}
