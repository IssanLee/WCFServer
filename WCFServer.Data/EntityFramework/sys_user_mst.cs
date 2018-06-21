namespace WCFServer.Data.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_user_mst
    {
        [StringLength(20)]
        public string id { get; set; }

        [Required]
        [StringLength(40)]
        public string name { get; set; }

        [StringLength(40)]
        public string password { get; set; }

        [Required]
        [StringLength(20)]
        public string group_no { get; set; }

        public DateTime? password_miss_date { get; set; }

        public int? password_miss_count { get; set; }

        [StringLength(20)]
        public string lock_status { get; set; }

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
