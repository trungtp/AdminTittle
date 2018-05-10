using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TittleAdmin.Model.DTO
{
    public class CustomLanguageTranslation
    {
        public long id { get; set; }
        public long key_id { get; set; }
        public string key { get; set; }
        public string label { get; set; }
        public string value { get; set; }
    }
}
