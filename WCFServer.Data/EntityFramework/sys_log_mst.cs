namespace WCFServer.Data.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_log_mst
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime date { get; set; }

        [StringLength(100)]
        public string thread { get; set; }

        [StringLength(100)]
        public string level { get; set; }

        [StringLength(200)]
        public string logger { get; set; }

        [Column("operator")]
        public int? _operator { get; set; }

        [Column(TypeName = "text")]
        public string message { get; set; }

        public int? action_type { get; set; }

        [StringLength(300)]
        public string operand { get; set; }

        [StringLength(20)]
        public string ip { get; set; }

        [StringLength(100)]
        public string machine_name { get; set; }

        [StringLength(50)]
        public string browser { get; set; }

        [Column(TypeName = "text")]
        public string location { get; set; }

        [Column(TypeName = "text")]
        public string exception { get; set; }
    }
}
