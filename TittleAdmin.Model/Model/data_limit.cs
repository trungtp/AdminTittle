namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.data_limit")]
    public partial class data_limit
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        public int user_id { get; set; }

        public int kid_id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public DateTime start_date { get; set; }

        public DateTime end_date { get; set; }

        public int duration { get; set; }

        public int repeat { get; set; }

        [Column("data_limit")]
        public long data_limit1 { get; set; }

        public long reset_data { get; set; }

        public int percent { get; set; }

        [Required]
        [StringLength(255)]
        public string unit { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }
    }
}
