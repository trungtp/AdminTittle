namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.zone")]
    public partial class zone
    {
        [Key]
        public int zone_id { get; set; }

        [Column(TypeName = "char")]
        [Required]
        [StringLength(2)]
        public string country_code { get; set; }

        [Required]
        [StringLength(35)]
        public string zone_name { get; set; }
    }
}
