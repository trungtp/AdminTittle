using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TittleAdmin.Model.DTO
{
    public class CustomUserInfo
    {
        public long id { get; set; }
        public List<string> plans { get; set; }
        public List<CustomUserTask> tasks { get; set; }
    }

    public class CustomUserTask
    {
        public long id { get; set; }
        public string Name { get; set; }
        public int NumberInProgress { get; set; }
        public int Points { get; set; }
        public string Status { get; set; }
    }
}
