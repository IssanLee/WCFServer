/* 由DapperEntitiesCreate生成 */
using System;
using WCFServer.Data.DapperEx.Attributies;
namespace WCFServer.Data.DapperEx.Entities
{
    /// <summary>
    /// SysLanguageMst 【】
    /// </summary>
    [Table("sys_language_mst")]
    public class SysLanguageMst
    {

        /// <summary>
        /// LangNo 【】
        /// </summary>
        [Key, Column("lang_no")]
        public string LangNo { get; set; }

        /// <summary>
        /// LangName 【】
        /// </summary>
        [Column("lang_name")]
        public string LangName { get; set; }

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
