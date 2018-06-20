using System;

namespace WCFServer.Data.DapperEx.Attributies
{
    /// <summary>
    /// 主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class KeyAttribute : BaseAttribute
    {
        /// <summary>
        /// 是否为自动主键
        /// </summary>
        public bool CheckAutoId { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        public KeyAttribute()
        {
            this.CheckAutoId = false;
        }
        /// <summary>
        /// 构造方法 
        /// </summary>
        /// <param name="checkAutoId">是否为自动主键</param>
        public KeyAttribute(bool checkAutoId)
        {
            this.CheckAutoId = checkAutoId;
        }
    }
}
