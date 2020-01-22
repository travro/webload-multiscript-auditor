using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using WMSA.Entities.Interfaces;

namespace WMSA_DAL.Models
{
    public partial class Transaction : ITransaction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Transaction()
        {
            Requests = new HashSet<Request>();
        }

        [Key, Column("id")]
        public int Id { get; set; }

        [Column("trans_nm_id")]
        public int? trans_nm_id { get; set; }

        [Column("script_id")]
        public int? script_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request> Requests { get; set; }

        public virtual Script Script { get; set; }

        public virtual TransactionName TransactionName { get; set; }

        [NotMapped]
        public string Name
        {
            get => TransactionName.trans_name;
            set => TransactionName.trans_name = value;
        }
        ICollection<IRequest> ITransaction.Requests
        {
            get => Requests as ICollection<IRequest>;
            set => Requests = value as ICollection<Request>;
        }
    }
}
