namespace WCFServer.Data.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_server_info_mst
    {
        public int id { get; set; }

        [Required]
        [StringLength(15)]
        public string ip { get; set; }

        [Required]
        [StringLength(40)]
        public string point { get; set; }

        public string remark { get; set; }

        public int? row_version { get; set; }

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
