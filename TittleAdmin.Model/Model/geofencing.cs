namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.geofencing")]
    public partial class geofencing
    {
        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public int kid_id { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string landmark { get; set; }

        [Required]
        [StringLength(255)]
        public string longitude { get; set; }

        [Required]
        [StringLength(255)]
        public string latitude { get; set; }

        public double radius { get; set; }

        public int start_time { get; set; }

        public int end_time { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string repeat { get; set; }

        [Column(TypeName = "date")]
        public DateTime date { get; set; }

        [Column(TypeName = "enum")]
        [Required]
        [StringLength(65532)]
        public string indicate { get; set; }

        [Column(TypeName = "enum")]
        [Required]
        [StringLength(65532)]
        public string period_type { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }

        [Required]
        [StringLength(255)]
        public string timezone { get; set; }

        [Required]
        [StringLength(255)]
        public string address { get; set; }

        [Required]
        [StringLength(255)]
        public string kid_ids { get; set; }

        public int user_id { get; set; }
    }
}
