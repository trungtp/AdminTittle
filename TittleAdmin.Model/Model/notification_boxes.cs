namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.notification_boxes")]
    public partial class notification_boxes
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Column(TypeName = "uint")]
        public long type { get; set; }

        [Required]
        [StringLength(255)]
        public string kid_token { get; set; }

        [Column(TypeName = "uint")]
        public long device_id { get; set; }

        [Required]
        [StringLength(50)]
        public string device_type { get; set; }

        [Required]
        [StringLength(255)]
        public string message { get; set; }

        [Column(TypeName = "uint")]
        public long task_id { get; set; }

        public int seen { get; set; }

        [Required]
        [StringLength(255)]
        public string unread { get; set; }

        public int kid_id_ref { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }
    }
}
