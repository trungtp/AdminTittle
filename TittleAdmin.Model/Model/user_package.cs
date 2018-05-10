namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.user_package")]
    public partial class user_package
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Column(TypeName = "uint")]
        public long user_id { get; set; }

        [Column(TypeName = "uint")]
        public long package_id { get; set; }

        [Required]
        [StringLength(255)]
        public string type { get; set; }

        [Column(TypeName = "uint")]
        public long unit { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime expired_at { get; set; }

        public byte expired { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }
    }
}
