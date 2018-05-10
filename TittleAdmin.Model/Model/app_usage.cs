namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.app_usage")]
    public partial class app_usage
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        public int kid_id { get; set; }

        [Required]
        [StringLength(255)]
        public string app_name { get; set; }

        public int type { get; set; }

        public int time_usage { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }

        [Column(TypeName = "date")]
        public DateTime date_used { get; set; }
    }
}
