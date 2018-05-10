using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TittleAdmin.Model.DTO
{
    public class CustomAddOnFeature
    {
        public string PackageName { get; set; }
        public int CurrentUser { get; set; }
        public int ActiveUser { get; set; }
        public int ExpiredUser { get; set; }
    }
}
