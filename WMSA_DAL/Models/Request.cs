namespace WMSA_DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Request
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Request()
        {
            Correlations = new HashSet<Correlation>();
        }

        public int id { get; set; }

        public int? verb_id { get; set; }

        [StringLength(256)]
        public string req_parameters { get; set; }

        public int? trans_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Correlation> Correlations { get; set; }

        public virtual RequestVerb RequestVerb { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
