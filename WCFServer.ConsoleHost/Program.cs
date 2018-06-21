using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using WCFServer.Common.Log4Net;
using WCFServer.Manager.Config;
using WCFServer.Manager.Provider;

namespace WCFServer.ConsoleHost
{
    class Program
    {
        // 消息多语言资源管理类变量
        public static ResourceManager langRes;

        static void Main(string[] args)
        {
            Log4Net.InitLogger();
            SetLang(System.Globalization.CultureInfo.CurrentCulture.Name);
            //Console.WriteLine("WCF服务启动中...");
            Log4Net.Info(typeof(Program), "WCF服务启动中...", true);

            List<ServiceType> serviceTypeList = new List<ServiceType>();
            ServiceType serviceType = new ServiceType
            {
                IntfType = typeof(Service.Demo.Contract.IService),
                ImplType = typeof(Service.Demo.Implement.Service),
                WcfConfig = new WcfConfig
                {
                    IP = "localhost",
                    Port = "1028",
                    Endpoint = "json"
                },
                LogAction = Console.WriteLine
            };
            //serviceTypeList.Add(serviceType);
            serviceTypeList = ServiceProvider.Instance.ServiceTypesProvider();
            ServiceProvider.Instance.AddService(serviceTypeList);
            Console.WriteLine("按Q键退出程序...");
            while (Console.ReadLine() != "Q")
                continue;
        }

        /// <summary>
        /// 国际化语言
        /// </summary>
        /// <param name="lang"></param>
        static public void SetLang(string lang)
        {
            langRes = new ResourceManager("WCFHost.Properties.Resource_" + lang, Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 错误、提示、警告信息取得
        /// </summary>
        /// <param name="msgId">消息ID</param>
        /// <param name="msgPars">参数</param>
        /// <returns></returns>
        public static string FormatMsg(string msgId, params string[] msgPars)
        {
            if (string.IsNullOrEmpty(msgId)) return "";

            string retMsg = Program.langRes.GetString(msgId);
            if (string.IsNullOrEmpty(retMsg)) return msgId;

            if (msgPars == null || msgPars.Length == 0)
            {
                return retMsg;
            }
            return String.Format(retMsg, msgPars);
        }
    }
}
