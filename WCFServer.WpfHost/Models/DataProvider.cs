using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WCFServer.Common;
using WCFServer.Manager;
using WCFServer.WPFApp.Models.Data;

namespace WCFServer.WPFApp.Models
{
    /// <summary>
    /// 数据提供类 单例模式
    /// </summary>
    public class DataProvider
    {
        private DataProvider() { }

        /// <summary>
        /// 数据提供类实例(单例)
        /// </summary>
        public static DataProvider Instance => Singleton<DataProvider>.Instance ?? (Singleton<DataProvider>.Instance = new DataProvider());

        public static List<InterfaceInfo> ServiceInfos { get; set; }

        public void InitData(Action<InterfaceInfo> action)
        {
            if (ServiceInfos != null) return;

            ServiceInfos = new List<InterfaceInfo>();
            foreach (var serviceInfo in ServiceManager.Instance.ServiceProvider())
            {
                InterfaceInfo intfInfo = new InterfaceInfo
                {
                    ServiceName = serviceInfo.Config.ServiceName,
                    ServiceAddress = string.Format(serviceInfo.Config.RomoteFormat, serviceInfo.Config.IP, serviceInfo.Config.Port, serviceInfo.ImplType.Name),
                    ServiceStatus = false,
                    ServiceDetail = serviceInfo,
                    Action = action
                };
                ServiceInfos.Add(intfInfo);
            }

        }

        
    }
}
