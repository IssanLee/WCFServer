/* 由DapperEntitiesCreate生成 */
using System;
using WCFServer.Data.DapperEx.Attributies;
namespace WCFServer.Data.DapperEx.Entities
{
    /// <summary>
    /// SysGroupMst 【】
    /// </summary>
    [Table("sys_group_mst")]
    public class SysGroupMst
    {

        /// <summary>
        /// GroupNo 【】
        /// </summary>
        [Key, Column("group_no")]
        public string GroupNo { get; set; }

        /// <summary>
        /// GroupName 【】
        /// </summary>
        [Column("group_name")]
        public string GroupName { get; set; }

        /// <summary>
        /// Description 【】
        /// </summary>
        [Column("description")]
        public string Description { get; set; }

        /// <summary>
        /// Status 【】
        /// </summary>
        [Column("status")]
        public string Status { get; set; }

        /// <summary>
        /// UpdateUser 【】
        /// </summary>
        [Column("update_user")]
        public string UpdateUser { get; set; }

        /// <summary>
        /// UpdateTime 【】
        /// </summary>
        [Column("update_time")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// CreateUser 【】
        /// </summary>
        [Column("create_user")]
        public string CreateUser { get; set; }

        /// <summary>
        /// CreateTime 【】
        /// </summary>
        [Column("create_time")]
        public DateTime CreateTime { get; set; }
      }
}
