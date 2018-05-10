namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.payment_histories")]
    public partial class payment_histories
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Column(TypeName = "uint")]
        public long user_id { get; set; }

        [Column(TypeName = "uint")]
        public long package_id { get; set; }

        public decimal amount { get; set; }

        [Required]
        [StringLength(255)]
        public string currency { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string payment_details { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }
    }
}
