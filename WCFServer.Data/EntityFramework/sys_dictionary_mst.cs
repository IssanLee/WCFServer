namespace WCFServer.Data.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sys_dictionary_mst
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string parent_type { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string type { get; set; }

        [Required]
        [StringLength(20)]
        public string lang_no { get; set; }

        [Required]
        [StringLength(50)]
        public string dic_content { get; set; }

        public string description { get; set; }

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
