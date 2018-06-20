namespace WCFServer.Data.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_group_mst
    {
        [Key]
        [StringLength(20)]
        public string group_no { get; set; }

        [Required]
        [StringLength(40)]
        public string group_name { get; set; }

        public string description { get; set; }

        [Required]
        [StringLength(10)]
        public string status { get; set; }

        [Required]
        [StringLength(20)]
        public string update_user { get; set; }

        public DateTime update_time { get; set; }

        [Required]
        [StringLength(20)]
        public string create_user { get; set; }

        public DateTime create_time { get; set; }
    }
}
