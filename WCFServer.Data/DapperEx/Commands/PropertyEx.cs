using System;

namespace WCFServer.Data.DapperEx.Commands
{
    [Serializable]
    public class PropertyEx : Property
    {
        #region 公开属性

        public string DbName { get; set; }
        public bool IsKey { get; set; }
        public string Comments { get; set; }

        #region 虚拟属性相关
        public bool IsVirtual { get; set; }
        public string[] DbForeignKey { get; set; }
        public string[] DbOnKey { get; set; }
        public string DbForeignTable { get; set; }

        #endregion

        #endregion
    }
}
