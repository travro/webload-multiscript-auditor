namespace WMSA_DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Script
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Script()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int id { get; set; }

        [StringLength(50)]
        public string script_name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? recording_date { get; set; }

        public int? test_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual Test Test { get; set; }
    }
}
