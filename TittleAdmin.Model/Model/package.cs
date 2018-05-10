namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.packages")]
    public partial class package
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [Required]
        [StringLength(255)]
        public string type { get; set; }

        public decimal amount { get; set; }

        [Column(TypeName = "uint")]
        public long duration_length { get; set; }

        [Required]
        [StringLength(255)]
        public string duration_unit { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }

        [Required]
        [StringLength(255)]
        public string currency { get; set; }

        [Column(TypeName = "uint")]
        public long unit { get; set; }

        [Required]
        [StringLength(255)]
        public string appstore_id { get; set; }

        [Required]
        [StringLength(255)]
        public string google_play_id { get; set; }

        [Column(TypeName = "uint")]
        public long addon_id { get; set; }

        [Column(TypeName = "uint")]
        public long version { get; set; }
    }
}
