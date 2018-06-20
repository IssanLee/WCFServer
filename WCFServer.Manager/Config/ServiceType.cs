using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer.Manager.Config
{
    public class ServiceType
    {
        /// <summary>
        /// 接口类型
        /// </summary>
        public Type IntfType { get; set; }

        /// <summary>
        /// 接口实现类类型
        /// </summary>
        public Type ImplType { get; set; }

        /// <summary>
        /// Action委托
        /// </summary>
        public Action<string> LogAction { get; set; }

        /// <summary>
        /// 服务相关配置
        /// </summary>
        public WcfConfig WcfConfig { get; set; }
    }
}
