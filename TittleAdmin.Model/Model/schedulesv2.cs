namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.schedulesv2")]
    public partial class schedulesv2
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Column(TypeName = "uint")]
        public long kid_id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public DateTime start_time { get; set; }

        public DateTime end_time { get; set; }

        public int repeat { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string options { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string extra { get; set; }

        [Column(TypeName = "uint")]
        public long disabled { get; set; }

        [Column(TypeName = "uint")]
        public long enabled { get; set; }

        public int state { get; set; }

        public int status_trigger { get; set; }

        public int app_version { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }

        [Required]
        [StringLength(255)]
        public string time_zone { get; set; }

        public sbyte all_day { get; set; }

        public int all_app { get; set; }

        public int noti_before_start { get; set; }
    }
}
