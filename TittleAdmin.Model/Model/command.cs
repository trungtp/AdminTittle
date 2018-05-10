namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.commands")]
    public partial class command
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Required]
        [StringLength(255)]
        public string udid { get; set; }

        [Column("command", TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string command1 { get; set; }

        [Column("params", TypeName = "text")]
        [StringLength(65535)]
        public string _params { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? run_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string extra { get; set; }
    }
}
