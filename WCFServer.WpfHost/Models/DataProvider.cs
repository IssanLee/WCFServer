using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WCFServer.Common;
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

        public static List<ServiceInfo> ServiceInfos { get; set; }

        public void InitData(Action<ServiceInfo> action)
        {
            if (ServiceInfos != null) return;
            ServiceInfos = new List<ServiceInfo>
            {
                new ServiceInfo() { ServiceName = "A接口", ServiceAddress = "http:localhost:1028/A/json/", ServiceStatus = true, Action = action},
                new ServiceInfo() { ServiceName = "B接口", ServiceAddress = "http:localhost:1028/B/json/", ServiceStatus = false, Action = action},
                new ServiceInfo() { ServiceName = "C接口", ServiceAddress = "http:localhost:1028/C/json/", ServiceStatus = false, Action = action},
                new ServiceInfo() { ServiceName = "D接口", ServiceAddress = "http:localhost:1028/D/json/", ServiceStatus = false, Action = action},
                new ServiceInfo() { ServiceName = "E接口", ServiceAddress = "http:localhost:1028/E/json/", ServiceStatus = false, Action = action},
                new ServiceInfo() { ServiceName = "F接口", ServiceAddress = "http:localhost:1028/F/json/", ServiceStatus = true, Action = action},
                new ServiceInfo() { ServiceName = "G接口", ServiceAddress = "http:localhost:1028/G/json/", ServiceStatus = true, Action = action},
            };

        }

        
    }
}
