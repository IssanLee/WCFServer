using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace WCFServer.Data.DapperEx.Commands
{
    [Serializable]
    public class Command
    {

        #region 公开属性

        /// <summary>
        /// SQL键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 指定如何解释命令字符串
        /// </summary>
        public Nullable<CommandType> CommandType { get; set; }

        /// <summary>
        /// SQL脚本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// SQL参数
        /// </summary>
        public List<Parameter> Parameters { get; set; }

        /// <summary>
        /// SQL参数
        /// </summary>
        public DynamicParameters DynamicParameters { get; set; }

        #endregion

        #region 构造函数

        public Command()
        {
            DynamicParameters = new DynamicParameters();
        }

        #endregion

    }
}
