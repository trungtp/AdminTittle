using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;

namespace TittleAdmin.Model.DTO
{
    public class CustomPerkTask
    {
        public long id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Key")]
        [Remote("IsPerkTaskExist", "UserTasks", AdditionalFields = "id")]
        public string key { get; set; }
        [Required]
        [StringLength(255)]
        [Display(Name = "Name")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Number To Finish")]
        public int number_to_finish { get; set; }
        [Required]
        [Display(Name = "Score")]
        public int score { get; set; }
        [StringLength(255)]
        public string icon { get; set; }
        public HttpPostedFileBase image { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}
