using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer.Service.PLC
{
    public class PLC : IPLC
    {
        public string GetData()
        {
            return "请求的是:PLC.GetData接口";
        }
    }
}
