namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.commands_android")]
    public partial class commands_android
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        public int kid_id { get; set; }

        public int device_id { get; set; }

        [Required]
        [StringLength(255)]
        public string package { get; set; }

        public int status { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }
    }
}
