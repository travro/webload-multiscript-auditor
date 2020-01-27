using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using WMSA.Entities.Interfaces;

namespace WMSA_DAL.Models
{
    public partial class Request: IRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Request()
        {
            Correlations = new HashSet<Correlation>();
        }

        [Key, Column("id")]
        public int Id { get; set; }

        [Column("verb_id")]
        public int? verb_id { get; set; }

        [StringLength(256), Column("req_parameters")]
        public string Parameters { get; set; }

        [Column("trans_id")]
        public int? trans_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Correlation> Correlations { get; set; }
        public virtual RequestVerb RequestVerb { get; set; }
        public virtual Transaction Transaction { get; set; }
        IEnumerable<ICorrelation> IRequest.Correlations
        {
            get => Correlations;
            set => Correlations = value as ICollection<Correlation>;
        }
        [NotMapped]
        string IRequest.Verb { get => RequestVerb.verb; set { RequestVerb.verb = value; } }
    }
}
