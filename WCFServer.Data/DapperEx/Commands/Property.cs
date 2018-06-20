using System;
using System.Data;

namespace WCFServer.Data.DapperEx.Commands
{
    [Serializable]
    public class Property
    {
        #region 公开属性

        public string Name { get; set; }

        public DbType DbType { get; set; }

        public string NativeType { get; set; }

        public byte Precision { get; set; }

        public int Scale { get; set; }

        public int Size { get; set; }

        #endregion

        #region 重写方法

        public override string ToString()
        {
            return string.Format("Name:{0},DbType:{1},NativeType:{2},Precision:{3},Scale:{4},Size:{5}",
                Name, DbType, NativeType, Precision, Scale, Size);
        }

        #endregion
    }
}
