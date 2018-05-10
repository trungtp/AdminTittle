namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.password_resets")]
    public partial class password_resets
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(255)]
        public string email { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(255)]
        public string token { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "timestamp")]
        public DateTime created_at { get; set; }
    }
}
