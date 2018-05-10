namespace TittleAdmin.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ironhutc_tittle.reminders")]
    public partial class reminder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public reminder()
        {
            comments = new HashSet<comment>();
        }

        [Column(TypeName = "uint")]
        public long id { get; set; }

        [Column(TypeName = "uint")]
        public long parent_id { get; set; }

        [Column(TypeName = "uint")]
        public long kid_id { get; set; }

        [Required]
        [StringLength(255)]
        public string task_name { get; set; }

        [Required]
        [StringLength(255)]
        public string status { get; set; }

        public DateTime? due_date { get; set; }

        [Required]
        [StringLength(255)]
        public string repeat { get; set; }

        public DateTime? end_repeat { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime updated_at { get; set; }

        [Required]
        [StringLength(255)]
        public string group_id { get; set; }

        public sbyte clone { get; set; }

        [Required]
        [StringLength(255)]
        public string timezone { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment> comments { get; set; }

        public virtual kid kid { get; set; }

        public virtual user user { get; set; }
    }
}
