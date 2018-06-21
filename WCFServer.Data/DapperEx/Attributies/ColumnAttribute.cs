using System;

namespace WCFServer.Data.DapperEx.Attributies
{
    /// <summary>
    /// 列字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ColumnAttribute : BaseAttribute
    {
        public ColumnAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
            { }
            Name = name;
        }

        /// <summary>
        /// 自增长
        /// </summary>
        public bool AutoIncrement { get; set; }
        public ColumnAttribute()
        {
            AutoIncrement = false;
        }
        /// <summary>
        /// 是否是自增长
        /// </summary>
        /// <param name="autoIncrement"></param>
        public ColumnAttribute(bool autoIncrement)
        {
            AutoIncrement = autoIncrement;
        }
    }
}
