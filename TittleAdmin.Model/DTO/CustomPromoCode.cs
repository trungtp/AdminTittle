using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TittleAdmin.Model.DTO
{
    public class CustomPromoCode
    {
        public long id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Code ID")]
        [Remote("IsPromoCodeExist", "PromoCode", AdditionalFields = "id")]
        public string CodeID { get; set; }
        
        [StringLength(255)]
        public string type { get; set; }

        public string Value { get; set; }

        public PromoValueType promoValueType { get; set; }


        [Required]
        [StringLength(65535)]
        [DataType(DataType.MultilineText)]
        public string Rules { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public string EndDate { get; set; }

        [Required]
        [StringLength(65535)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
        [StringLength(255)]
        [Display(Name = "Type")]
        public string TypeValue { get; set; }

        public PromoTypes promoType { get; set; }

        public string Quantity { get; set; }

        public string Status { get; set; }
    }
}
