using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using WMSA.Entities.Interfaces;

namespace WMSA_DAL.Models
{
    public partial class Script: IScript
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Script()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key, Column("id")]
        public int Id { get; set; }

        [StringLength(50), Column("script_name")]
        public string Name { get; set; }

        [Column("recording_date")]
        public DateTime RecordedDate { get; set; }

        public int? test_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual Test Test { get; set; }

        [NotMapped]
        public string TestName { get => Test.test_name; set => Test.test_name = value; }
        [NotMapped]
        public string BuildVersion { get => Test.build_version; set => Test.build_version = value; }
        ICollection<ITransaction> IScript.Transactions
        {
            get => Transactions as ICollection<ITransaction>;
            set => Transactions = value as ICollection<Transaction>;
        }
    }
}
