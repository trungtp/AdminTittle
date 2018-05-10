using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TittleAdmin.Model.DTO
{
    public class CustomNewLanguage
    {
        [Required]
        public string locale { get; set; }
        [Required]
        public string langLabel { get; set; }
        public List<CustomLanguage> AddedLanguages { get; set; }
        public Dictionary<string, string> AvailableLanguages { get; set; }
    }
}
