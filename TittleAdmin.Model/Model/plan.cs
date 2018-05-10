namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.plans")]
    public partial class plan
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [Required]
        [StringLength(255)]
        public string multiple_kids { get; set; }

        [Required]
        [StringLength(255)]
        public string multiple_login { get; set; }

        [Required]
        [StringLength(255)]
        public string amount { get; set; }

        public int schedule { get; set; }

        public int location { get; set; }

        public int geo_location { get; set; }

        public int geo_lankmark { get; set; }

        public int tasks { get; set; }

        public int web_tracking { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }
    }
}
