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

            foreach (var item in pairs)
            {
                ServiceType serviceType = new ServiceType
                {
                    IntfType = item.Key,
                    ImplType = item.Value,
                    WcfConfig = new WcfConfig
                    {
                        IP = ip,
                        Port = port.ToString(),
                        Endpoint = endpoint
                    }
                };
                serviceTypeList.Add(serviceType);
            }

            #region 2.本地与数据库融合
            #endregion

            using (var db = new DapperExContext())
            {
                var data = db.Query<SysServerInfoMst>().ToList();

                #region paris为空 || data为空
                if (data.Count == 0 || pairs.Count == 0)
                {
                    #region @1.paris为空,data全部删除
                    if (pairs.Count == 0)
                        foreach (var item in data)
                        {
                            var delete = db.Delete<SysServerInfoMst>(x => x.IntfName == item.IntfName && x.ImplName == item.ImplName);
                        }
                    #endregion

                    #region @2.data为空,paris全部插入
                    if (data.Count == 0)
                        foreach (var item in pairs)
                        {
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
                    #endregion
                }
                #endregion 
                #region 都不为空
                else
                {
                    #region @1.添加 ==> 以paris为主,data中没有的添加
                    List<SysServerInfoMst> serverInfosAdd = new List<SysServerInfoMst>();
                    foreach (var p in pairs)
                    {
                        foreach (var d in data.Where(x => x.IntfName == p.Key.Name && x.ImplName == p.Value.Name))
                        {
                            serverInfosAdd.Add(d);
                        }
                    }
                    

                    #endregion

                    #region @2.删除 ==> 以data为主,paris中没有的删除
                    #endregion
                }
                #endregion

                // 增加 => 利用pairs查找db
                if (data.Count != 0)
                {
                    foreach (var d in data)
                    {
                        foreach (var item in pairs.Where(x => x.Key.Name != d.IntfName || x.Value.Name != d.ImplName))
                        {
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
                            //var insert = db.Insert<SysServerInfoMst>(sysServerInfo);
                        }
                    }
                }
                else
                {
                    foreach (var item in pairs)
                    {
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
                        //var insert = db.Insert<SysServerInfoMst>(sysServerInfo);
                    }
                }

                // 删除 => 利用db查找pairs
                foreach (var kv in pairs)
                {
                    foreach (var item in data.Where(x => x.IntfName != kv.Key.Name || x.ImplName != kv.Value.Name))
                    {
                        //var delete = db.Delete<SysServerInfoMst>(x => x.IntfName == item.IntfName && x.ImplName == item.ImplName);
                    }
                }
            }

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
