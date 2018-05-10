namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.location_landmark")]
    public partial class location_landmark
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        public int kid_id { get; set; }

        [Required]
        [StringLength(255)]
        public string location { get; set; }

        public int radius { get; set; }

        public DateTime datetime_trigger { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }
    }
}
