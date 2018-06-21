using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using WCFServer.Data.DapperEx.Commands;

namespace WCFServer.Data.DapperEx.Context
{
    public class DapperExContext : IDisposable
    {

        private ISession session;
        private CommandBuilder commandBuilder;

        public DapperExContext() : this ("DataContext")
        { }

        public DapperExContext(string connectionStringName)
        {
            session = new Session(connectionStringName);
            commandBuilder = new CommandBuilder(session.ParamPrefix);
        }

        public void Dispose()
        {
            if (session != null) session.Dispose();
        }

        #region Query

        #region SQL文
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="command">查询脚本</param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(string command)
        {
            return session.Connection.Query(command, null, session.Transaction, false, session.CommandTimeout, null);
        }

        /// <summary>
        /// 查询 [带参数]
        /// </summary>
        /// <param name="command">查询脚本</param>
        /// <param name="dynParameters">命令参数</param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(string command, Object dynParameters)
        {
            return session.Connection.Query(command, dynParameters, session.Transaction, false, session.CommandTimeout, null);
        }

        /// <summary>
        /// 查询 [带参数并指明如何解释命令字符串]
        /// </summary>
        /// <param name="command">查询脚本</param>
        /// <param name="dynParameters">命令参数</param>
        /// <param name="commandType">脚本类型</param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(string command, Object dynParameters, CommandType? commandType)
        {
            return session.Connection.Query(command, dynParameters, session.Transaction, false, session.CommandTimeout, commandType);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageInfo">页信息</param>
        /// <param name="command">查询脚本</param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(PageInfo pageInfo, string command)
        {
            string sql = BuildPageInfo(command, pageInfo);
            return session.Connection.Query(sql, null, session.Transaction, false, session.CommandTimeout, null);
        }

        /// <summary>
        /// 分页查询 [带参数]
        /// </summary>
        /// <param name="pageInfo">页信息</param>
        /// <param name="command">查询脚本</param>
        /// <param name="dynParameters">命令参数</param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(PageInfo pageInfo, string command, Object dynParameters)
        {
            string sql = BuildPageInfo(command, pageInfo);
            return session.Connection.Query(sql, dynParameters, session.Transaction, false, session.CommandTimeout, null);
        }

        /// <summary>
        /// 分页查询 [带参数并指明如何解释命令字符串]
        /// </summary>
        /// <param name="pageInfo">页信息</param>
        /// <param name="command">查询脚本</param>
        /// <param name="dynParameters">命令参数</param>
        /// <param name="commandType">脚本类型</param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(PageInfo pageInfo, string command, Object dynParameters, CommandType? commandType)
        {
            string sql = BuildPageInfo(command, pageInfo);
            return session.Connection.Query(sql, dynParameters, session.Transaction, false, session.CommandTimeout, commandType);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="command">查询脚本</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string command)
        {
            return session.Connection.Query<T>(command, null, session.Transaction, false, session.CommandTimeout, null);
        }

        /// <summary>
        /// 查询 [带参数]
        /// </summary>
        /// <param name="command">查询脚本</param>
        /// <param name="dynParameters">命令参数</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string command, Object dynParameters)
        {
            return session.Connection.Query<T>(command, dynParameters, session.Transaction, false, session.CommandTimeout, null);
        }

        /// <summary>
        /// 查询 [带参数并指明如何解释命令字符串]
        /// </summary>
        /// <param name="command">查询脚本</param>
        /// <param name="dynParameters">命令参数</param>
        /// <param name="commandType">脚本类型</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string command, Object dynParameters, CommandType? commandType)
        {
            return session.Connection.Query<T>(command, dynParameters, session.Transaction, false, session.CommandTimeout, commandType);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageInfo">页信息</param>
        /// <param name="command">查询脚本</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(PageInfo pageInfo, string command)
        {
            string sql = BuildPageInfo(command, pageInfo);
            return session.Connection.Query<T>(sql, null, session.Transaction, false, session.CommandTimeout, null);
        }

        /// <summary>
        /// 分页查询 [带参数]
        /// </summary>
        /// <param name="pageInfo">页信息</param>
        /// <param name="command">查询脚本</param>
        /// <param name="dynParameters">命令参数</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(PageInfo pageInfo, string command, Object dynParameters)
        {
            string sql = BuildPageInfo(command, pageInfo);
            return session.Connection.Query<T>(sql, dynParameters, session.Transaction, false, session.CommandTimeout, null);
        }

        /// <summary>
        /// 分页查询 [带参数并指明如何解释命令字符串]
        /// </summary>
        /// <param name="pageInfo">页信息</param>
        /// <param name="command">查询脚本</param>
        /// <param name="dynParameters">命令参数</param>
        /// <param name="commandType">脚本类型</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(PageInfo pageInfo, string command, Object dynParameters, CommandType? commandType)
        {
            string sql = BuildPageInfo(command, pageInfo);
            return session.Connection.Query<T>(sql, dynParameters, session.Transaction, false, session.CommandTimeout, commandType);
        }
        #endregion

        #region Lambda

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">具体Entity泛型</typeparam>
        /// <returns></returns>
        public IEnumerable<T> Query<T>() where T : class
        {
            return Query<T>(null, null, null, null, false);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">具体Entity泛型</typeparam>
        /// <param name="Include">是否left join</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(bool Include) where T : class
        {
            return Query<T>(null, Include);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">具体Entity泛型</typeparam>
        /// <param name="predicate">Lambda表达式</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return Query<T>(predicate, false);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">具体Entity泛型</typeparam>
        /// <param name="predicate">Lambda表达式</param>
        /// <param name="Include">是否left join</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(Expression<Func<T, bool>> predicate, bool Include) where T : class
        {
            return Query<T>(predicate, null, null, Include);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">具体Entity泛型</typeparam>
        /// <param name="predicate">Lambda表达式</param>
        /// <param name="orderBy">orderby</param>
        /// <param name="Include">是否left join</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(Expression<Func<T, bool>> predicate, OrderBy orderBy, bool Include) where T : class
        {
            return Query<T>(predicate, null, orderBy, Include);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">具体Entity泛型</typeparam>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="predicate">Lambda表达式</param>
        /// <param name="Include">是否left join</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(PageInfo pageInfo, Expression<Func<T, bool>> predicate, bool Include) where T : class
        {
            return Query<T>(pageInfo, predicate, null, null, Include);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">具体Entity泛型</typeparam>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="predicate">Lambda表达式</param>
        /// <param name="orderBy">orderby</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(PageInfo pageInfo, Expression<Func<T, bool>> predicate, OrderBy orderBy) where T : class
        {
            return Query<T>(pageInfo, predicate, orderBy, false);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">具体Entity泛型</typeparam>
        /// <param name="predicate">Lambda表达式</param>
        /// <param name="dynParameters">脚本参数</param>
        /// <param name="orderBy">orderby</param>
        /// <param name="Include">是否left join</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(Expression<Func<T, bool>> predicate, IDictionary<string, object> dynParameters, OrderBy orderBy, bool Include) where T : class
        {
            return Query<T>(null, predicate, dynParameters, orderBy, Include);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">具体Entity泛型</typeparam>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="predicate">Lambda表达式</param>
        /// <param name="orderBy">orderby</param>
        /// <param name="Include">是否left join</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(PageInfo pageInfo, Expression<Func<T, bool>> predicate, OrderBy orderBy, bool Include) where T : class
        {
            return Query<T>(pageInfo, predicate, null, orderBy, Include);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">具体Entity泛型</typeparam>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="predicate">Lambda表达式</param>
        /// <param name="dynParameters">脚本参数</param>
        /// <param name="Include">是否left join</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(PageInfo pageInfo, Expression<Func<T, bool>> predicate, IDictionary<string, object> dynParameters, bool Include) where T : class
        {
            return Query<T>(pageInfo, predicate, null, null, Include);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T">具体Entity泛型</typeparam>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="predicate">Lambda表达式</param>
        /// <param name="dynParameters">脚本参数</param>
        /// <param name="orderBy">orderBy</param>
        /// <param name="Include">是否left join</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(PageInfo pageInfo, Expression<Func<T, bool>> predicate, IDictionary<string, object> dynParameters, OrderBy orderBy, bool Include) where T : class
        {
            Command cmd = new Command();
            
            cmd = commandBuilder.Build<T>(CommandBuilder.Select, predicate, dynParameters, orderBy);

            if (pageInfo != null)
                cmd.Text = BuildPageInfo(cmd.Text, pageInfo);
            return Query<T>(cmd.Text, cmd.DynamicParameters, cmd.CommandType);
        }
        #endregion

        #region Count
        /// <summary>
        /// 查询数量
        /// </summary>
        /// <typeparam name="T">具体Entity泛型</typeparam>
        /// <param name="predicate">Lambda表达式</param>
        /// <returns></returns>
        public int Count<T>(Expression<Func<T, bool>> predicate = null) where T : class
        {
            return Count<T>(predicate, null);
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <typeparam name="T">具体Entity泛型</typeparam>
        /// <param name="dynParameters">脚本参数</param>
        /// <returns></returns>
        public int Count<T>(IDictionary<string, object> dynParameters) where T : class
        {
            return Count<T>(null, dynParameters);
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <typeparam name="T">具体Entity泛型</typeparam>
        /// <param name="predicate">Lambda表达式</param>
        /// <param name="dynParameters">脚本参数</param>
        /// <returns></returns>
        public int Count<T>(Expression<Func<T, bool>> predicate, IDictionary<string, object> dynParameters) where T : class
        {
            Command cmd = commandBuilder.Build<T>(CommandBuilder.Count, predicate, dynParameters, null);
            return session.Connection.QueryFirst<int>(cmd.Text, cmd.DynamicParameters, session.Transaction, session.CommandTimeout, cmd.CommandType);
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="command">脚本</param>
        /// <param name="dynParameters">脚本参数</param>
        /// <param name="commandType">脚本类型</param>
        /// <returns></returns>
        public int Count(string command, Object dynParameters = null, CommandType? commandType = null)
        {
            return session.Connection.QueryFirst<int>(BuildCount(command), dynParameters, session.Transaction, session.CommandTimeout, commandType);
        }
        #endregion

        #region QueryFirstOrDefault

        public T QueryFirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return QueryFirstOrDefault<T>(predicate, null, false);
        }

        public T QueryFirstOrDefault<T>(Expression<Func<T, bool>> predicate, bool Include = false) where T : class
        {
            return QueryFirstOrDefault<T>(predicate, null, Include);
        }

        public T QueryFirstOrDefault<T>(Expression<Func<T, bool>> predicate = null, IDictionary<string, object> dynParameters = null, bool Include = false) where T : class
        {
            Command cmd = new Command();
            //if (Include)
            //    cmd = commandBuilder.Build<T>(CommandBuilder.SelectJoin, predicate, dynParameters, null);
            //else
                cmd = commandBuilder.Build<T>(CommandBuilder.Select, predicate, dynParameters, null);
            return QueryFirstOrDefault<T>(cmd.Text, cmd.DynamicParameters, cmd.CommandType);
        }

        public dynamic QueryFirstOrDefault(string command, Object dynParameters = null, CommandType? commandType = null)
        {
            return session.Connection.QueryFirstOrDefault(command, dynParameters, session.Transaction, session.CommandTimeout, commandType);
        }

        public T QueryFirstOrDefault<T>(string command, Object dynParameters = null, CommandType? commandType = null)
        {
            return session.Connection.QueryFirstOrDefault<T>(command, dynParameters, session.Transaction, session.CommandTimeout, commandType);
        }

        #endregion 

        #region QueryMultiple

        public SqlMapper.GridReader QueryMultiple(string command, Object dynParameters = null, CommandType? commandType = null)//DynamicParameters
        {
            return session.Connection.QueryMultiple(command, dynParameters, session.Transaction, session.CommandTimeout, commandType);
        }

        #endregion

        #region QueryDataTable
        public DataTable QueryDataTable(string command, Object dynParameters = null, CommandType? commandType = null)
        {
            IDataReader reader = session.Connection.ExecuteReader(command, dynParameters, session.Transaction, session.CommandTimeout, commandType);
            DataTable table = new DataTable();
            table.Load(reader);
            return table;
        }

        #endregion

        #endregion

        #region Insert
        /// <summary>
        /// 插入
        /// </summary>
        /// <typeparam name="T">entities泛型</typeparam>
        /// <param name="TEntity">entities泛型</param>
        /// <returns></returns>
        public int Insert<T>(T TEntity) where T : class, new()
        {
            Command cmd = commandBuilder.Build<T>(CommandBuilder.Insert, TEntity);
            int result = session.Connection.Execute(cmd.Text, TEntity, session.Transaction, session.CommandTimeout);
            return result;
        }
        #endregion

        #region Delete

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">entities泛型</typeparam>
        /// <param name="TEntity">entities泛型</param>
        /// <returns></returns>
        public int Delete<T>(T TEntity) where T : class, new()
        {
            Command cmd = commandBuilder.Build<T>(CommandBuilder.DeleteByKey, TEntity);
            int result = session.Connection.Execute(cmd.Text, TEntity, session.Transaction, session.CommandTimeout);
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">entities泛型</typeparam>
        /// <param name="predicate">lambda参数</param>
        /// <returns></returns>
        public int Delete<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return Execute<T>(CommandBuilder.Delete, predicate);
        }
        #endregion

        #region Update

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TEntity"></param>
        /// <returns></returns>
        public int Update<T>(T TEntity) where T : class, new()
        {
            Command cmd = commandBuilder.Build<T>(CommandBuilder.UpdateByKey, TEntity);
            int result = session.Connection.Execute(cmd.Text, TEntity, session.Transaction, session.CommandTimeout);
            return result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updater"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Update<T>(Expression<Func<T, T>> updater, Expression<Func<T, bool>> predicate) where T : class
        {
            Command cmd = commandBuilder.Build<T>(updater, predicate);
            return Execute(cmd.Text, cmd.DynamicParameters, cmd.CommandType);
        }
        #endregion

        #region Execute

        /// <summary>
        /// 执行脚本，返回影响行数
        /// </summary>
        /// <param name="command">查询脚本</param>
        /// <param name="dynParameters">命令参数</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        public int Execute(string command, Object dynParameters = null, CommandType? commandType = null)
        {
            return session.Connection.Execute(command, dynParameters, session.Transaction, session.CommandTimeout, commandType);
        }

        /// <summary>
        /// 执行脚本，返回影响行数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdName"></param>
        /// <param name="predicate"></param>
        /// <param name="dynParameters"></param>
        /// <returns></returns>
        private int Execute<T>(string cmdName, Expression<Func<T, bool>> predicate = null, DynamicParameters dynParameters = null) where T : class
        {
            Command cmd = commandBuilder.Build<T>(cmdName, predicate, null, null);
            return Execute(cmd.Text, cmd.DynamicParameters, cmd.CommandType);
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 分页查询拼接
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        private String BuildPageInfo(String sql, PageInfo pageInfo)
        {
            return string.Format(@"SELECT * FROM (
                                                    SELECT ROWNUM as NUM, A.* FROM ( {0} ) A
                                                    WHERE ROWNUM <= {1}
                                                 )
                                                WHERE NUM >= {2}", sql, pageInfo.CurrentPage * pageInfo.PageSize, (pageInfo.CurrentPage - 1) * pageInfo.PageSize + 1);
        }

        /// <summary>
        /// 计算数量拼接
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private String BuildCount(String sql)
        {
            return string.Format(@"SELECT COUNT(*)  FROM ( {0} )", sql);
        }
        #endregion
    }
}
