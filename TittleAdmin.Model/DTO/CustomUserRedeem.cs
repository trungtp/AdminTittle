using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TittleAdmin.Model.DTO
{
    public class CustomUserRedeem
    {
        public long id { get; set; }
        public string UserName { get; set; }
        public string Redeem { get; set; }
        public string DateRedeem { get; set; }
        public string Status { get; set; }
        public string ActionName { get; set; }
    }
}
