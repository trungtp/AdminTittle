namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.task_schedules")]
    public partial class task_schedules
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Column(TypeName = "uint")]
        public long kid_id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [Column(TypeName = "uint")]
        public long start_time { get; set; }

        [Column(TypeName = "uint")]
        public long end_time { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string repeat { get; set; }

        [Column(TypeName = "date")]
        public DateTime? date { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string options { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string extra { get; set; }

        [Column(TypeName = "uint")]
        public long enabled { get; set; }

        [Column(TypeName = "uint")]
        public long disabled { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }

        public int state { get; set; }

        [Column(TypeName = "enum")]
        [Required]
        [StringLength(65532)]
        public string period_type { get; set; }

        public int status_trigger { get; set; }

        public int app_version { get; set; }
    }
}
