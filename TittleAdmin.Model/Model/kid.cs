namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.kids")]
    public partial class kid
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public kid()
        {
            reminders = new HashSet<reminder>();
        }

        [Column(TypeName = "uint")]
        public long id { get; set; }

        public int user_id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [Required]
        [StringLength(255)]
        public string image { get; set; }

        [Required]
        [StringLength(255)]
        public string model { get; set; }

        [Required]
        [StringLength(255)]
        public string code { get; set; }

        public bool active { get; set; }

        public int expired { get; set; }

        public int all_app { get; set; }

        [Required]
        [StringLength(255)]
        public string kid_token { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? last_request { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? last_seen { get; set; }

        [Required]
        [StringLength(255)]
        public string timezone { get; set; }

        public int version { get; set; }

        [Required]
        [StringLength(255)]
        public string phone_version { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reminder> reminders { get; set; }
    }
}
