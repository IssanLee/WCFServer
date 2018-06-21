using System;

namespace WCFServer.Data.DapperEx.Commands
{
    [Serializable]
    public class TableType
    {
        #region 公开属性

        public string TableName { get; set; }

        public string TypeFullName { get; set; }

        #endregion
    }
}
