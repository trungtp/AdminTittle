namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.orders")]
    public partial class order
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Column(TypeName = "uint")]
        public long user_id { get; set; }

        [Column(TypeName = "uint")]
        public long package_id { get; set; }

        [Column(TypeName = "uint")]
        public long promo_code_id { get; set; }

        [Column(TypeName = "uint")]
        public long payment_history_id { get; set; }

        public decimal amount { get; set; }

        [Required]
        [StringLength(255)]
        public string currency { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }
    }
}
