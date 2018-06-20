/* 由DapperEntitiesCreate生成 */
using System;
using WCFServer.Data.DapperEx.Attributies;
namespace WCFServer.Data.DapperEx.Entities
{
    /// <summary>
    /// SysUserMst 【系统用户表】
    /// </summary>
    [Table("sys_user_mst")]
    public class SysUserMst
    {

        /// <summary>
        /// Id 【】
        /// </summary>
        [Key, Column("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name 【】
        /// </summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// Password 【】
        /// </summary>
        [Column("password")]
        public string Password { get; set; }

        /// <summary>
        /// GroupNo 【】
        /// </summary>
        [Column("group_no")]
        public string GroupNo { get; set; }

        /// <summary>
        /// PasswordMissDate 【】
        /// </summary>
        [Column("password_miss_date")]
        public DateTime PasswordMissDate { get; set; }

        /// <summary>
        /// PasswordMissCount 【】
        /// </summary>
        [Column("password_miss_count")]
        public int PasswordMissCount { get; set; }

        /// <summary>
        /// LockStatus 【】
        /// </summary>
        [Column("lock_status")]
        public string LockStatus { get; set; }

        /// <summary>
        /// RowVersion 【】
        /// </summary>
        [Column("row_version")]
        public int RowVersion { get; set; }

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
