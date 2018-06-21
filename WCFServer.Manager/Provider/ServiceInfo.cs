using System;
using System.ServiceModel;

namespace WCFServer.Manager.Provider
{
    public class ServiceInfo
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
        public ServiceConfig Config { get; set; }

        /// <summary>
        /// 服务主机
        /// </summary>
        public ServiceHost Host { get; set; }
    }
}
