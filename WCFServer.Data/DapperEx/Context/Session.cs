using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace WCFServer.Data.DapperEx.Context
{
    public class Session : ISession
    {
        #region 私有变量

        /// <summary>
        /// 数据源连接
        /// </summary>
        private IDbConnection dbConnection;

        /// <summary>
        /// 创建提供程序对数据源类的实例
        /// </summary>
        private DbProviderFactory dbFactory;

        /// <summary>
        /// 事务 提交(true)或终止(false)的标志
        /// </summary>
        private bool consistent = false;

        /// <summary>
        /// 事务
        /// </summary>
        private IDbTransaction dbTransaction = null;

        /// <summary>
        /// 事务所是否开启
        /// </summary>
        protected bool isOpenTran = false;

        /// <summary>
        /// 参数前缀
        /// </summary>
        private string paramPrefix = "@";

        /// <summary>
        /// ProviderName
        /// </summary>
        private string providerName = "System.Data.SqlClient";

        /// <summary>
        /// 数据库类型 【默认SqlServer】
        /// </summary>
        private DBType dbType = DBType.SqlServer;

        #endregion

        #region 构造函数

        /// <summary>
        /// SqlMapSession
        /// </summary>
        /// <param name="dataSource">数据源Info</param>
        public Session(string connectionStringName)
        {
            this.ConnectionStringName = connectionStringName;
        }

        #endregion

        #region 接口实现

        /// <summary>
        /// 连接符
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// 超时设置
        /// </summary>
        public int CommandTimeout { get; set; } = 900;

        /// <summary>
        /// 数据源的连接
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                if (dbConnection == null) CreateConnection();
                return dbConnection;
            }
        }

        /// <summary>
        /// 参数前缀
        /// </summary>
        public string ParamPrefix
        {
            get { return paramPrefix; }
            set
            {
                string dbtype = (dbFactory == null ? dbConnection.GetType() : dbFactory.GetType()).Name;

                // 使用类型名判断
                if (dbtype.StartsWith("MySql")) dbType = DBType.MySql;
                else if (dbtype.StartsWith("SqlCe")) dbType = DBType.SqlServerCE;
                else if (dbtype.StartsWith("Npgsql")) dbType = DBType.PostgreSQL;
                else if (dbtype.StartsWith("Oracle")) dbType = DBType.Oracle;
                else if (dbtype.StartsWith("SQLite")) dbType = DBType.SQLite;
                else if (dbtype.StartsWith("System.Data.SqlClient.")) dbType = DBType.SqlServer;
                // else try with provider name
                else if (providerName.IndexOf("MySql", StringComparison.InvariantCultureIgnoreCase) >= 0) dbType = DBType.MySql;
                else if (providerName.IndexOf("SqlServerCe", StringComparison.InvariantCultureIgnoreCase) >= 0) dbType = DBType.SqlServerCE;
                else if (providerName.IndexOf("Npgsql", StringComparison.InvariantCultureIgnoreCase) >= 0) dbType = DBType.PostgreSQL;
                else if (providerName.IndexOf("Oracle", StringComparison.InvariantCultureIgnoreCase) >= 0) dbType = DBType.Oracle;
                else if (providerName.IndexOf("SQLite", StringComparison.InvariantCultureIgnoreCase) >= 0) dbType = DBType.SQLite;

                if (dbType == DBType.MySql && dbConnection != null && dbConnection.ConnectionString != null && dbConnection.ConnectionString.IndexOf("Allow User Variables=true") >= 0)
                    paramPrefix = "?";
                if (dbType == DBType.Oracle)
                    paramPrefix = ":";
            }
        }

        /// <summary>
        /// 事务所
        /// </summary>
        public IDbTransaction Transaction
        {
            get { return dbTransaction; }
        }

        /// <summary>
        /// 事务所是否打开
        /// </summary>
        public bool IsOpenTran
        {
            get { return isOpenTran; }
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        public void CreateConnection()
        {
            if (dbConnection == null)
            {
                var connStr = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
                if (!string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[ConnectionStringName].ProviderName))
                    providerName = ConfigurationManager.ConnectionStrings[ConnectionStringName].ProviderName;
                else
                    throw new Exception("ConnectionStrings中没有配置提供程序ProviderName！");
                dbFactory = DbProviderFactories.GetFactory(providerName);
                dbConnection = dbFactory.CreateConnection();
                dbConnection.ConnectionString = connStr;
            }
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        public void OpenConnection()
        {
            if (dbConnection == null) CreateConnection();
            if (dbConnection.State != ConnectionState.Open) dbConnection.Open();
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnection()
        {
            if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
            dbConnection = null;
        }

        /// <summary>
        /// 打开连接并开始一个事务所
        /// </summary>
        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.Unspecified);
        }

        /// <summary>
        /// 打开连接并开始一个事务所并指明等级
        /// </summary>
        /// <param name="isolationLevel">事务隔离级别</param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (isOpenTran) return;

            OpenConnection();
            dbTransaction = dbConnection.BeginTransaction(isolationLevel);
            isOpenTran = true;
        }

        /// <summary>
        /// 提交事务并关闭关联的连接
        /// </summary>
        public void CommitTransaction()
        {
            CommitTransaction(true);
        }

        /// <summary>
        /// 提交数据库事务
        /// </summary>
        /// <param name="closeConnection">是否关闭连接</param>
        public void CommitTransaction(bool closeConnection)
        {
            if (!isOpenTran) return;

            if (dbTransaction != null)
            {
                dbTransaction.Commit();
                dbTransaction.Dispose();
                dbTransaction = null;
            }

            isOpenTran = false;
            if (closeConnection) CloseConnection();
        }

        /// <summary>
        /// 回滚事务并关闭关联的连接
        /// </summary>
        public void RollBackTransaction()
        {
            this.RollBackTransaction(true);
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="closeConnection">是否关闭连接</param>
        public void RollBackTransaction(bool closeConnection)
        {
            if (!isOpenTran) return;

            if (dbTransaction != null)
            {
                dbTransaction.Rollback();
                dbTransaction.Dispose();
                dbTransaction = null;
            }

            isOpenTran = false;
            if (closeConnection) CloseConnection();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (consistent)
                {
                    CommitTransaction();
                }
                else
                {
                    RollBackTransaction();
                }
            }
            finally
            {
                CloseConnection();
                isOpenTran = false;
                consistent = false;
            }
        }

        /// <summary>
        /// 完成事务
        /// </summary>
        public void Complete()
        {
            consistent = true;
        }
        #endregion
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DBType
    {
        SqlServer,
        SqlServerCE,
        MySql,
        PostgreSQL,
        Oracle,
        SQLite
    }
}
