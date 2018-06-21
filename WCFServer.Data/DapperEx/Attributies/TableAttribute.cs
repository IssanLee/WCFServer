using System;

namespace WCFServer.Data.DapperEx.Attributies
{
    /// <summary>
    /// 数据库表
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TableAttribute : BaseAttribute
    {
        public TableAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                // throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, DataAnnotationsResources.ArgumentIsNullOrWhitespace, "name"));
            }
            this.Name = name;
        }
    }
}
