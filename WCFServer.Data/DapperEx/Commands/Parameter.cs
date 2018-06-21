using System;
using System.Collections.Generic;
using System.Data;

namespace WCFServer.Data.DapperEx.Commands
{
    [Serializable]
    public class Parameter : Property
    {
        #region 公开属性

        public object Value { get; set; }

        public Nullable<ParameterDirection> Direction { get; set; }

        #endregion
    }

    public class ParameterComparer : IEqualityComparer<Parameter>
    {

        public bool Equals(Parameter x, Parameter y)
        {
            return x != null && y != null && string.Compare(x.Name, y.Name, true) == 0;
        }

        public int GetHashCode(Parameter obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
