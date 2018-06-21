/* 由DapperEntitiesCreate生成 */
using System;
using WCFServer.Data.DapperEx.Attributies;
namespace WCFServer.Data.DapperEx.Entities
{
    /// <summary>
    /// SysLogMst 【】
    /// </summary>
    [Table("sys_log_mst")]
    public class SysLogMst
    {

        /// <summary>
        /// Id 【】
        /// </summary>
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Date 【】
        /// </summary>
        [Column("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Thread 【】
        /// </summary>
        [Column("thread")]
        public string Thread { get; set; }

        /// <summary>
        /// Level 【】
        /// </summary>
        [Column("level")]
        public string Level { get; set; }

        /// <summary>
        /// Logger 【】
        /// </summary>
        [Column("logger")]
        public string Logger { get; set; }

        /// <summary>
        /// Operator 【】
        /// </summary>
        [Column("operator")]
        public int Operator { get; set; }

        /// <summary>
        /// Message 【】
        /// </summary>
        [Column("message")]
        public string Message { get; set; }

        /// <summary>
        /// ActionType 【】
        /// </summary>
        [Column("action_type")]
        public int ActionType { get; set; }

        /// <summary>
        /// Operand 【】
        /// </summary>
        [Column("operand")]
        public string Operand { get; set; }

        /// <summary>
        /// Ip 【】
        /// </summary>
        [Column("ip")]
        public string Ip { get; set; }

        /// <summary>
        /// MachineName 【】
        /// </summary>
        [Column("machine_name")]
        public string MachineName { get; set; }

        /// <summary>
        /// Browser 【】
        /// </summary>
        [Column("browser")]
        public string Browser { get; set; }

        /// <summary>
        /// Location 【】
        /// </summary>
        [Column("location")]
        public string Location { get; set; }

        /// <summary>
        /// Exception 【】
        /// </summary>
        [Column("exception")]
        public string Exception { get; set; }
      }
}
