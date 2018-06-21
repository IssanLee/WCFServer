using System;

namespace WCFServer.Data.DapperEx.Attributies
{
    /// <summary>
    /// 外部键
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ForeignAttribute : BaseAttribute
    {
        public string[] Key { get; set; }
        public Type Table { get; set; }
    }
}
