namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.users")]
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            reminders = new HashSet<reminder>();
        }

        [Column(TypeName = "uint")]
        public long id { get; set; }

        public int role { get; set; }

        public int active { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [Required]
        [StringLength(255)]
        public string first_name { get; set; }
        
        [StringLength(255)]
        public string last_name { get; set; }

        [Required]
        [StringLength(255)]
        public string email { get; set; }

        [Required]
        [StringLength(20)]
        public string phone { get; set; }

        [Required]
        [StringLength(60)]
        public string password { get; set; }

        [StringLength(100)]
        public string remember_token { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }

        [Required]
        [StringLength(255)]
        public string activation_code { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? activated_at { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string auth_token { get; set; }

        [Required]
        [StringLength(255)]
        public string multiple_login { get; set; }

        [Required]
        [StringLength(255)]
        public string multiple_kids { get; set; }

        [Required]
        [StringLength(255)]
        public string account_type { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? upgrade_expired_at { get; set; }

        [Required]
        [StringLength(255)]
        public string timezone { get; set; }
        
        [StringLength(255)]
        public string id_facebook { get; set; }

        [Required]
        [StringLength(255)]
        public string country { get; set; }

        [Required]
        [StringLength(255)]
        public string code { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reminder> reminders { get; set; }
    }
}
