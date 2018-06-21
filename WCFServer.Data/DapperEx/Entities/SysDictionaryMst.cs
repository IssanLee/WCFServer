/* 由DapperEntitiesCreate生成 */
using System;
using WCFServer.Data.DapperEx.Attributies;
namespace WCFServer.Data.DapperEx.Entities
{
    /// <summary>
    /// SysDictionaryMst 【】
    /// </summary>
    [Table("sys_dictionary_mst")]
    public class SysDictionaryMst
    {

        /// <summary>
        /// ParentType 【】
        /// </summary>
        [Key, Column("parent_type")]
        public string ParentType { get; set; }

        /// <summary>
        /// Type 【】
        /// </summary>
        [Key, Column("type")]
        public string Type { get; set; }

        /// <summary>
        /// LangNo 【】
        /// </summary>
        [Column("lang_no")]
        public string LangNo { get; set; }

        /// <summary>
        /// DicContent 【】
        /// </summary>
        [Column("dic_content")]
        public string DicContent { get; set; }

        /// <summary>
        /// Description 【】
        /// </summary>
        [Column("description")]
        public string Description { get; set; }

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
