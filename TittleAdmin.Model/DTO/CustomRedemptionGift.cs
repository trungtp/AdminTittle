using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TittleAdmin.Model.DTO
{
    public class CustomRedemptionGift
    {
        public long id { get; set; }
        [Required]
        [StringLength(255)]
        [Display(Name = "Name")]
        [Remote("IsRedemptionGiftExist", "Redeem", AdditionalFields = "id")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Type")]
        public int type { get; set; }
        [Required]
        [StringLength(255)]
        [Display(Name = "Frequency")]
        public string frequency { get; set; }
        [Required]
        [Display(Name = "Points")]
        public int points { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
}
