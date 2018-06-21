using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer.Service.Login
{
    [ServiceContract]
    public interface ILogin : IBaseContract
    {
        [OperationContract]
        [WebGet]
        string GetData();
    }
}
