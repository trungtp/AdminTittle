namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.jobs")]
    public partial class job
    {
        [Column(TypeName = "ubigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        [StringLength(255)]
        public string queue { get; set; }

        [Required]
        [StringLength(1073741823)]
        public string payload { get; set; }

        public byte attempts { get; set; }

        public byte reserved { get; set; }

        [Column(TypeName = "uint")]
        public long? reserved_at { get; set; }

        [Column(TypeName = "uint")]
        public long available_at { get; set; }

        [Column(TypeName = "uint")]
        public long created_at { get; set; }
    }
}
