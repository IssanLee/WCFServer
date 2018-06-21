using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFServer.Service.Demo.Contract;

namespace WCFServer.Service.Demo.Implement
{
    public class Service : IService
    {
        public string GetData(int value)
        {
            return string.Format("你输入的是: {0}", value);
        }

        public string GetDataJson()
        {
            return "GetDataJson";
        }

        public string PostData(string name)
        {
            return "PostData" + name;
        }
    }
}
