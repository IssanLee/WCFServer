using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFServer.Common;

namespace WCFServer.Manager.Config
{
    /// <summary>
    /// 服务相关配置
    /// </summary>
    public class WcfConfig
    {
        /// <summary>
        /// 服务端IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 通信端口号
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        ///  终结点地址
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// 连接地址规则   
        /// http://localhost:1024/json/XXService/
        /// http://localhost:1024/XXService/
        /// </summary>
        //public string RomoteFormat { get; set; }
        public string RomoteFormat = "http://{0}:{1}/{2}";
    }
}
