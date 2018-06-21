using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer.Common.Base
{
    public class TypeHelper
    {
        /// <summary>
        /// 获取传入接口下所有的子类
        /// </summary>
        /// <param name="interfaceType">接口</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetIntfChildrens(Type interfaceType)
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
    }
}
