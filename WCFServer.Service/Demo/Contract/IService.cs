using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer.Service.Demo.Contract
{
    [ServiceContract]
    public interface IService
    {
        /*
            1、在契约中，接口请求格式为RequestFormat = WebMessageFormat.Json，所以在做post请求的时候，要在headers中指定Content-Type为application/json 
            2、契约中的BodyStyle有四个枚举值，分别为： 
                WebMessageBodyStyle.Bare ：对请求和返回都不包装 
                WebMessageBodyStyle.Wrapped：对请求和返回都包装 
                WebMessageBodyStyle.WrappedRequest：对请求包装，对返回不包装 
                WebMessageBodyStyle.WrappedResponse：对返回包装，对请求不包装 
         */


        [OperationContract]
        [WebGet]
        string GetData(int value);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string GetDataJson();

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string PostData(string name);
    }
}
