namespace WMSA_DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Test
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Test()
        {
            Scripts = new HashSet<Script>();
        }

        [Key]
        public int id { get; set; }

        [StringLength(50)]
        public string test_name { get; set; }

        [StringLength(50)]
        public string build_version { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Script> Scripts { get; set; }
    }
}
