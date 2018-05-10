namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.devices")]
    public partial class device
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        public int deviceable_id { get; set; }

        [Required]
        [StringLength(255)]
        public string deviceable_type { get; set; }

        public int device_id { get; set; }

        [Required]
        [StringLength(255)]
        public string device_type { get; set; }

        [Required]
        [StringLength(255)]
        public string os { get; set; }

        [Required]
        [StringLength(255)]
        public string token { get; set; }

        [Required]
        [StringLength(255)]
        public string mobile_token { get; set; }

        [Required]
        [StringLength(255)]
        public string imei { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }

        [Required]
        [StringLength(255)]
        public string model_number { get; set; }

        [Required]
        [StringLength(255)]
        public string manufacturer { get; set; }

        [StringLength(255)]
        public string udid { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string mdm { get; set; }

        [StringLength(1073741823)]
        public string restrictions { get; set; }

        [Required]
        [StringLength(10)]
        public string language { get; set; }
    }
}
