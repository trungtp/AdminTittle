namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.maps")]
    public partial class map
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        public int kid_id { get; set; }

        [Required]
        [StringLength(255)]
        public string kid_name { get; set; }

        [Required]
        [StringLength(20)]
        public string latitude { get; set; }

        [Required]
        [StringLength(20)]
        public string longtitude { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public DateTime last_request { get; set; }

        public int device_id { get; set; }

        public int push { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string device_ids { get; set; }
    }
}
