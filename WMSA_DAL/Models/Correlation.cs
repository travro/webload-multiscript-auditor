using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using WMSA.Entities.Interfaces;

namespace WMSA_DAL.Models
{
    public partial class Correlation : ICorrelation
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [StringLength(128), Column("corr_rule")]
        public string Rule { get; set; }

        [StringLength(256), Column("original_val")]
        public string OriginalValue { get; set; }

        [Column("req_id")]
        public int? req_id { get; set; }

        public virtual Request Request { get; set; }
    }
}
