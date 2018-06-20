using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer.Service.Login
{
    public class Login : ILogin
    {
        public string GetData()
        {
            return "请求的是:Login.GetData接口";
        }
    }
}
