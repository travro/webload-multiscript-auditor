namespace WMSA_DAL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Correlation
    {
        public int id { get; set; }

        [StringLength(128)]
        public string corr_rule { get; set; }

        [StringLength(256)]
        public string original_val { get; set; }

        public int? req_id { get; set; }

        public virtual Request Request { get; set; }
    }
}
