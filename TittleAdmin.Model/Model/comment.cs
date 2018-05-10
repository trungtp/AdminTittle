namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.comments")]
    public partial class comment
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Column("comment")]
        [Required]
        [StringLength(255)]
        public string comment1 { get; set; }

        [Column(TypeName = "uint")]
        public long reminder_id { get; set; }

        [Column("object")]
        public sbyte _object { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string image { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }

        public virtual reminder reminder { get; set; }
    }
}
