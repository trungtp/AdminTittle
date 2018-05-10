namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.keys")]
    public partial class key
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Column("key")]
        [Required]
        [StringLength(255)]
        public string key1 { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string label { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }
    }
}
