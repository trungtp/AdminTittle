namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.notifications")]
    public partial class notification
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [Required]
        [StringLength(255)]
        public string data { get; set; }

        public DateTime? time { get; set; }

        public DateTime? next_notification { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string content { get; set; }

        [Required]
        [StringLength(255)]
        public string status { get; set; }

        [Required]
        [StringLength(255)]
        public string type { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }
    }
}
