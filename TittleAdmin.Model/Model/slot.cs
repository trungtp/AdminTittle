namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.slots")]
    public partial class slot
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Column(TypeName = "uint")]
        public long user_id { get; set; }

        public int? kid_id { get; set; }

        public int? device_id { get; set; }

        public int? package_id { get; set; }

        public int? user_package_id { get; set; }

        public DateTime? expired_at { get; set; }

        public sbyte expired { get; set; }

        [Required]
        [StringLength(20)]
        public string type { get; set; }

        public int position { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }
    }
}
