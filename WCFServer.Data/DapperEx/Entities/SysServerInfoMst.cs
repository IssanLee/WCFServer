/* 由DapperEntitiesCreate生成 */
using System;
using WCFServer.Data.DapperEx.Attributies;
namespace WCFServer.Data.DapperEx.Entities
{
    /// <summary>
    /// SysServerInfoMst 【】
    /// </summary>
    [Table("sys_server_info_mst")]
    public class SysServerInfoMst
    {

        /// <summary>
        /// IntfName 【】
        /// </summary>
        [Key, Column("intf_name")]
        public string IntfName { get; set; }

        /// <summary>
        /// ImplName 【】
        /// </summary>
        [Key, Column("impl_name")]
        public string ImplName { get; set; }

        /// <summary>
        /// Ip 【】
        /// </summary>
        [Column("ip")]
        public string Ip { get; set; }

        /// <summary>
        /// Port 【】
        /// </summary>
        [Column("port")]
        public int Port { get; set; }

        /// <summary>
        /// Endpoint 【】
        /// </summary>
        [Column("endpoint")]
        public string Endpoint { get; set; }

        /// <summary>
        /// ServiceName 【】
        /// </summary>
        [Column("service_name")]
        public string ServiceName { get; set; }

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
