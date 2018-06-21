using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using WCFServer.Common;
using WCFServer.Common.Base;
using WCFServer.Data.DapperEx.Context;
using WCFServer.Data.DapperEx.Entities;
using WCFServer.Manager.Provider;
using WCFServer.Service;

namespace WCFServer.Manager
{
    /// <summary>
    /// 服务管理类
    /// </summary>
    public class ServiceManager
    {
        /// <summary>
        /// 私有化构造方法
        /// </summary>
        private ServiceManager() { }

        /// <summary>
        /// 服务管理类实例(单例)
        /// </summary>
        public static ServiceManager Instance => Singleton<ServiceManager>.Instance ?? (Singleton<ServiceManager>.Instance = new ServiceManager());
        
        /// <summary>
        /// 服务接口提供器
        /// </summary>
        /// <returns></returns>
        public List<ServiceInfo> ServiceProvider()
        {
            List<ServiceInfo> serviceInfoList = new List<ServiceInfo>();

            #region 0.默认数据
            string ip = "localhost";
            int port = 1028;
            string endpoint = "json";
            #endregion

            #region 1.获取所有的服务接口及其实现类
            Dictionary<Type, Type> pairs = new Dictionary<Type, Type>();
            var types = TypeHelper.GetIntfChildrens(typeof(IBaseContract));
            bool isPair = true;
            Type type1 = null, type2 = null;
            foreach (var type in types)
            {
                if (isPair)
                {
                    type1 = type;
                    isPair = false;
                }
                else
                {
                    type2 = type;
                    pairs.Add(type1, type2);
                    isPair = true;
                }
            }
            #endregion

            #region 2.本地与数据库融合
            using (var db = new DapperExContext())
            {
                var data = db.Query<SysServerInfoMst>().ToList();

                // 【删除】找出DB中存在,实际接口不存在的数据,并删除DB的该条记录
                foreach (var item in data)
                {
                    if (pairs.Count(x => x.Key.Name == item.IntfName && x.Value.Name == item.ImplName) == 0)
                    {
                        var delete = db.Delete<SysServerInfoMst>(x => x.IntfName == item.IntfName && x.ImplName == item.ImplName);
                    }
                }

                // 【新增】数据到DB、【设置】DB数据到本地
                foreach (var item in pairs)
                {
                    ServiceInfo serviceInfo = new ServiceInfo
                    {
                        IntfType = item.Key,
                        ImplType = item.Value,
                        LogAction = Console.WriteLine
                    };

                    var tmpPairs = data.Where(x => x.IntfName == item.Key.Name && x.ImplName == item.Value.Name).ToList();
                    if (tmpPairs.Count != 0)
                    {
                        serviceInfo.Config = new ServiceConfig
                        {
                            IP = tmpPairs.FirstOrDefault().Ip,
                            Port = tmpPairs.FirstOrDefault().Port.ToString(),
                            Endpoint = tmpPairs.FirstOrDefault().Endpoint,
                            ServiceName = tmpPairs.FirstOrDefault().ServiceName
                        };
                    }
                    else
                    {
                        serviceInfo.Config = new ServiceConfig
                        {
                            IP = ip,
                            Port = port.ToString(),
                            Endpoint = endpoint,
                            ServiceName = item.Value.Name + "服务接口"
                        };

                        SysServerInfoMst sysServerInfo = new SysServerInfoMst
                        {
                            IntfName = item.Key.Name,
                            ImplName = item.Value.Name,
                            ServiceName = item.Value.Name + "服务接口",
                            RowVersion = 1,
                            Status = "1",
                            CreateUser = "liweipeng",
                            UpdateUser = "liweipeng",
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now,
                            Ip = "localhost",
                            Port = 1028,
                            Endpoint = "json"
                        };
                        var insert = db.Insert<SysServerInfoMst>(sysServerInfo);
                    }
                    serviceInfoList.Add(serviceInfo);
                }
            }
            #endregion

            return serviceInfoList;
        }

        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="serviceInfoList">服务接口集合</param>
        public void OpenService(List<ServiceInfo> serviceInfoList)
        {
            foreach (var serviceInfo in serviceInfoList)
            {
                Action<string> logAction = serviceInfo.LogAction;
                logAction = logAction ?? (msg => { });
                string serviceName = serviceInfo.ImplType.Name;
                string endpointAddress = string.Format(serviceInfo.Config.RomoteFormat, serviceInfo.Config.IP, serviceInfo.Config.Port, serviceName);
                ServiceHost host = new ServiceHost(serviceInfo.ImplType, new Uri(endpointAddress));

                ServiceMetadataBehavior behavior = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
                if (behavior == null)
                {
                    behavior = new ServiceMetadataBehavior
                    {
                        #if DEBUG
                        HttpGetEnabled = true
                        #else
                        HttpGetEnabled = false
                        #endif
                    };
                    host.Description.Behaviors.Add(behavior);
                }
                ServiceEndpoint endpoint = host.AddServiceEndpoint(serviceInfo.IntfType, new WebHttpBinding(), serviceInfo.Config.Endpoint);
                // 设置wcf支持ajax调用,仅适用于WebHttpBinding
                //endpoint.Behaviors.Add(new WebScriptEnablingBehavior());
                WebHttpBehavior webHttpBehavior = new WebHttpBehavior
                {
                    #if DEBUG
                    HelpEnabled = true
                    #else
                    HelpEnabled = false
                    #endif
                };
                endpoint.Behaviors.Add(webHttpBehavior);
                host.Opened += delegate
                {
                    var log = $"{endpoint.Address}\t服务已启动\t{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                    logAction(log);
                };
                host.Open();
                serviceInfo.Host = host;
                //serviceHosts.Add(host);
            }
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        /// <param name="serviceInfoList">服务接口集合</param>
        public void CloseService(List<ServiceInfo> serviceInfoList)
        {
            foreach (var serviceInfo in serviceInfoList)
            {
                serviceInfo.Host.Closed += delegate
                {
                    var log = $"{serviceInfo.Config.ServiceName}\t服务已关闭\t{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                    serviceInfo.LogAction(log);
                };
                serviceInfo.Host.Close();
            }
        }
    }
}
