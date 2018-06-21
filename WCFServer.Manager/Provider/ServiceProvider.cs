using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Linq;
using WCFServer.Common;
using WCFServer.Manager.Config;
using WCFServer.Service;
using WCFServer.Data.DapperEx.Context;
using WCFServer.Data.DapperEx.Entities;

namespace WCFServer.Manager.Provider
{
    /// <summary>
    /// 服务提供类
    /// </summary>
    public class ServiceProvider
    {
        private ServiceProvider() { }

        /// <summary>
        /// 服务提供类实例(单例)
        /// </summary>
        public static ServiceProvider Instance => Singleton<ServiceProvider>.Instance ?? (Singleton<ServiceProvider>.Instance = new ServiceProvider());

        List<ServiceHost> serviceHosts = new List<ServiceHost>();

        /// <summary>
        /// ServiceType提供
        /// </summary>
        /// <returns></returns>
        public List<ServiceType> ServiceTypesProvider()
        {
            List<ServiceType> serviceTypeList = new List<ServiceType>();

            string ip = "localhost";
            int port = 1028;
            string endpoint = "json";

            #region 1.获取所有的服务接口及其实现类
            // type1[Key]:Intf, type2[value]:Impl 
            Dictionary<Type, Type> pairs = new Dictionary<Type, Type>();
            var types = GetType(typeof(IBaseContract));
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
                    ServiceType serviceType = new ServiceType
                    {
                        IntfType = item.Key,
                        ImplType = item.Value,
                        LogAction = Console.WriteLine
                    };

                    var tmpPairs = data.Where(x => x.IntfName == item.Key.Name && x.ImplName == item.Value.Name).ToList();
                    if (tmpPairs.Count != 0)
                    {
                        serviceType.WcfConfig = new WcfConfig
                        {
                            IP = tmpPairs.FirstOrDefault().Ip,
                            Port = tmpPairs.FirstOrDefault().Port.ToString(),
                            Endpoint = tmpPairs.FirstOrDefault().Endpoint
                        };
                    }
                    else
                    {
                        serviceType.WcfConfig = new WcfConfig
                        {
                            IP = ip,
                            Port = port.ToString(),
                            Endpoint = endpoint
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
                    serviceTypeList.Add(serviceType);
                }
            }
            #endregion

            return serviceTypeList;
        }

        private static IEnumerable<Type> GetType(Type interfaceType)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var t in type.GetInterfaces())
                    {
                        if (t == interfaceType)
                        {
                            yield return type;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="serviceTypes"></param>
        public void AddService(List<ServiceType> serviceTypeList)
        {
            foreach (var serviceType in serviceTypeList)
            {
                Action<string> logAction = serviceType.LogAction;
                logAction = logAction ?? (msg => { });
                string serviceName = serviceType.ImplType.Name;
                string endpointAddress = string.Format(serviceType.WcfConfig.RomoteFormat, serviceType.WcfConfig.IP, serviceType.WcfConfig.Port, serviceName);
                ServiceHost host = new ServiceHost(serviceType.ImplType, new Uri(endpointAddress));

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
                ServiceEndpoint endpoint = host.AddServiceEndpoint(serviceType.IntfType, new WebHttpBinding(), serviceType.WcfConfig.Endpoint);
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
                    var log = $"{endpoint.Address} 服务已启动\t{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                    logAction(log);
                };
                host.Open();
                serviceHosts.Add(host);
            }
        }
    }
}
