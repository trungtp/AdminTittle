using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TittleAdmin.Model.DTO
{
    public class CustomGridData
    {
        public int Total { get; set; }
        public int MaxParent { get; set; }
        public int MaxChild { get; set; }
        public List<CustomUser> Users { get; set; }
    }
}
