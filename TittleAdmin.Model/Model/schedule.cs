namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.schedules")]
    public partial class schedule
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        public int kid_id { get; set; }

        public int start_time { get; set; }

        public int end_time { get; set; }

        [Required]
        [StringLength(255)]
        public string repeat { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }

        [Column(TypeName = "uint")]
        public long enabled { get; set; }

        [Column(TypeName = "uint")]
        public long reverted { get; set; }
    }
}
