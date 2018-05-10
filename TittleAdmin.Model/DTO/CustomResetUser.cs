using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TittleAdmin.Model.DTO
{
    public class CustomResetUser
    {
        public long id { get; set; }
        public int active { get; set; }
        public DateTime updated_at { get; set; }
    }
}
