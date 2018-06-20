using System;

namespace WCFServer.Data.DapperEx.Attributies
{
    public abstract class BaseAttribute : Attribute
    {
        /// <summary>
        /// 别名，对应数据里面的名字
        /// </summary>
        public string Name { get; set; }
    }
}
