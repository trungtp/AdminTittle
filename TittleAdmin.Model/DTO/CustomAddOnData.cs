using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TittleAdmin.Model.DTO
{
    public class CustomAddOnData
    {
        public string PackageFullName { get; set; }
        public int CurrentTotal { get; set; }
        public int ActiveTotal { get; set; }
        public int ExpiredTotal { get; set; }
    }
}
