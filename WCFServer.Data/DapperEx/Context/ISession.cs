using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer.Data.DapperEx.Context
{
    public interface ISession : IDisposable
    {
        IDbConnection Connection { get; }

        IDbTransaction Transaction { get; }

        string ConnectionStringName { get; set; }

        int CommandTimeout { get; set; }

        string ParamPrefix { get; set; }

        bool IsOpenTran { get; }

        void Complete();

        void OpenConnection();

        void CloseConnection();

        void BeginTransaction();

        void BeginTransaction(IsolationLevel isolationLevel);

        void CommitTransaction();

        void CommitTransaction(bool closeConnection);

        void RollBackTransaction();

        void RollBackTransaction(bool closeConnection);
    }
}
