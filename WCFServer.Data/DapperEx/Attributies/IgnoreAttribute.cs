using System;

namespace WCFServer.Data.DapperEx.Attributies
{
    /// <summary>
    /// 忽略字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IgnoreAttribute : BaseAttribute
    {
    }
}
