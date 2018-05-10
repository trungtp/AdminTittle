namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.promo_codes")]
    public partial class promo_codes
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Required]
        [StringLength(255)]
        public string code { get; set; }

        [Required]
        [StringLength(255)]
        public string type { get; set; }

        public decimal value { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string rule { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime start_date { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime end_date { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string description { get; set; }

        [Required]
        [StringLength(255)]
        public string promo_type { get; set; }

        public int quantity { get; set; }
    }
}
