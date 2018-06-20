using System;
using System.Data;
using System.Data.SqlClient;

namespace WCFServer.Data.ADO.NET
{
    public class SQLServerDB : IDisposable
    {
        public static string ConnStr = "";

        public SqlConnection Conn = null;
        public SqlCommand Cmd = null;

        #region 构造方法
        /// <summary>
        /// SQLServerDB
        /// </summary>
        public SQLServerDB()
        {
            GetConn();
            GetSqlCommand();
        }
        /// <summary>
        /// SQLServerDB
        /// </summary>
        /// <param name="ConnectionString">连接字符串</param>
        public SQLServerDB(string ConnectionString)
        {
            try
            {
                ConnStr = ConnectionString;
                GetConn();
                GetSqlCommand();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public string constr()
        {
            return ConnStr;
        }

        /// <summary> 
        /// 返回connection对象 
        /// </summary> 
        /// <returns></returns> 
        public SqlConnection GetConn()
        {
            try
            {
                Conn = new SqlConnection(ConnStr);
                Conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Conn;
        }

        public void Dispose()
        {
            if (Cmd != null)
            {
                if (Cmd.Transaction != null)
                {
                    Cmd.Transaction.Dispose();
                }
                Cmd.Dispose();
            }
            if (Conn != null)
            {
                Conn.Close();
                Conn.Dispose();
            }
            GC.Collect();
        }

        /// <summary> 
        /// 运行SQL语句 
        /// </summary> 
        /// <param name="SQL"></param> 
        public void RunSql(string SQL)
        {
            Cmd.CommandText = SQL;
            try
            {
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string exm = ex.Message;
                throw new Exception(SQL);
            }
            return;
        }

        /// <summary> 
        /// 运行SQL语句 返回影响行数
        /// </summary> 
        /// <param name="SQL"></param> 
        public int ExecuteNonQuery(string SQL)
        {
            int i = 0;
            Cmd.CommandText = SQL;
            try
            {
                i = Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string exm = ex.Message;
                throw new Exception(SQL);
            }
            return i;
        }

        /// <summary> 
        /// 运行SQL语句返回DataReader 
        /// </summary> 
        /// <param name="SQL"></param> 
        /// <returns>SqlDataReader对象.</returns> 
        public SqlDataReader ExecuteReader(string SQL)
        {
            Cmd.CommandText = SQL;
            SqlDataReader Dr;
            try
            {
                Dr = Cmd.ExecuteReader(CommandBehavior.Default);
            }
            catch (Exception ex)
            {
                throw new Exception(SQL);
            }
            return Dr;
        }

        /*
        /// <summary> 
        /// 生成Command对象 
        /// </summary> 
        /// <param name="SQL"></param> 
        /// <param name="Conn"></param> 
        /// <returns></returns> 
        public SqlCommand GetSqlCommand(string SQL, SqlConnection Conn)
        {
            SqlCommand Cmd;
            Cmd = new SqlCommand(SQL, Conn);
            return Cmd;
        }

        /// <summary> 
        /// 生成Command对象 
        /// </summary> 
        /// <param name="SQL"></param> 
        /// <returns></returns> 
        public SqlCommand GetSqlCommand(string SQL)
        {
            SqlCommand Cmd;
            Cmd = new SqlCommand(SQL, Conn);
            return Cmd;
        }
        */

        /// <summary> 
        /// 生成Command对象 
        /// </summary> 
        /// <param name="SQL"></param> 
        /// <returns></returns> 
        public SqlCommand GetSqlCommand()
        {
            Cmd = new SqlCommand();
            Cmd.Connection = Conn;
            return Cmd;
        }

        /// <summary> 
        /// 生成Transaction对象 
        /// </summary> 
        /// <param name="SQL"></param> 
        /// <returns></returns> 
        public SqlTransaction GetSqlTransaction()
        {
            SqlTransaction tran = Conn.BeginTransaction();
            Cmd.Transaction = tran;
            return tran;
        }

        /// <summary> 
        /// Transaction Commit 
        /// </summary> 
        /// <param name="SQL"></param> 
        /// <returns></returns> 
        public void Commit()
        {
            if (Cmd.Transaction != null)
            {
                Cmd.Transaction.Commit();
            }
        }

        /// <summary> 
        /// Transaction Rollback
        /// </summary> 
        /// <param name="SQL"></param> 
        /// <returns></returns> 
        public void Rollback()
        {
            if (Cmd.Transaction != null)
            {
                Cmd.Transaction.Rollback();
            }
        }

        /// <summary> 
        /// 返回reader对象 
        /// </summary> 
        /// <param name="SQL"></param> 
        /// <returns></returns> 
        public SqlDataReader GetDataReader(string SQL)
        {
            Cmd.CommandText = SQL;

            SqlDataReader Dr;
            Dr = Cmd.ExecuteReader();
            return Dr;
        }

        /// <summary> 
        /// 返回adapter对象 
        /// </summary> 
        /// <param name="SQL"></param> 
        /// <returns></returns> 
        public SqlDataAdapter GetDataAdapter(string SQL)
        {
            SqlDataAdapter Da;
            Da = new SqlDataAdapter(SQL, Conn);
            return Da;
        }

        /// <summary> 
        /// 运行SQL语句,返回DataSet对象 
        /// </summary> 
        /// <param name="procName">SQL语句</param> 
        /// <param name="prams">DataSet对象</param> 
        public DataSet RunProc(string SQL, DataSet Ds)
        {
            SqlDataAdapter Da;
            //Da = CreateDa(SQL, Conn); 
            Da = new SqlDataAdapter(SQL, Conn);
            try
            {
                Da.Fill(Ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ds;
        }

        /// <summary> 
        /// 运行SQL语句,返回DataSet对象 
        /// </summary> 
        /// <param name="procName">SQL语句</param> 
        /// <param name="prams">DataSet对象</param> 
        /// <param name="dataReader">表名</param> 
        public DataSet RunProc(string SQL, DataSet Ds, string tablename)
        {
            SqlDataAdapter Da;
            Da = GetDataAdapter(SQL);
            try
            {
                Da.Fill(Ds, tablename);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ds;
        }

        /// <summary> 
        /// 运行SQL语句,返回DataSet对象 
        /// </summary> 
        /// <param name="procName">SQL语句</param> 
        /// <param name="prams">DataSet对象</param> 
        /// <param name="dataReader">表名</param> 
        public DataSet RunProc(string SQL, DataSet Ds, int StartIndex, int PageSize, string tablename)
        {
            SqlDataAdapter Da;
            Da = GetDataAdapter(SQL);
            try
            {
                Da.Fill(Ds, StartIndex, PageSize, tablename);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ds;
        }

        /// <summary> 
        /// 检验是否存在数据 
        /// </summary> 
        /// <returns></returns> 
        public bool ExistData(string SQL)
        {
            Cmd.CommandText = SQL;

            SqlDataReader Dr;
            Dr = Cmd.ExecuteReader();
            if (Dr.Read())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary> 
        /// 返回SQL语句执行结果的第一行第一列 
        /// </summary> 
        /// <returns>字符串</returns> 
        public string ReturnValue(string SQL)
        {
            string result;
            SqlDataReader Dr;
            try
            {
                Cmd.CommandText = SQL;

                Dr = Cmd.ExecuteReader();
                if (Dr.Read())
                {
                    result = Dr[0].ToString();
                    Dr.Close();
                }
                else
                {
                    result = "";
                    Dr.Close();
                }
            }
            catch
            {
                throw new Exception(SQL);
            }
            return result;
        }

        /// <summary> 
        /// 返回SQL语句第一列,第ColumnI列, 
        /// </summary> 
        /// <returns>字符串</returns> 
        public string ReturnValue(string SQL, int ColumnI)
        {
            string result;
            SqlDataReader Dr;
            try
            {
                Cmd.CommandText = SQL;
                Dr = Cmd.ExecuteReader();
            }
            catch
            {
                throw new Exception(SQL);
            }
            if (Dr.Read())
            {
                result = Dr[ColumnI].ToString();
            }
            else
            {
                result = "";
            }
            Dr.Close();
            return result;
        }

        /// <summary> 
        /// 生成一个存储过程使用的sqlcommand. 
        /// </summary> 
        /// <param name="procName">存储过程名.</param> 
        /// <param name="prams">存储过程入参数组.</param> 
        /// <returns>sqlcommand对象.</returns> 
        public SqlCommand GetCommand(string procName, SqlParameter[] prams)
        {
            SqlCommand Cmd = new SqlCommand(procName, Conn);
            Cmd.CommandType = CommandType.StoredProcedure;
            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                {
                    if (parameter != null)
                    {
                        Cmd.Parameters.Add(parameter);
                    }
                }
            }
            return Cmd;
        }

        /// <summary> 
        /// 为存储过程生成一个SqlCommand对象 
        /// </summary> 
        /// <param name="procName">存储过程名</param> 
        /// <param name="prams">存储过程参数</param> 
        /// <returns>SqlCommand对象</returns> 
        private SqlCommand GetCommand(string procName, SqlParameter[] prams, SqlDataReader Dr)
        {
            SqlCommand Cmd = new SqlCommand(procName, Conn);
            Cmd.CommandType = CommandType.StoredProcedure;
            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                    Cmd.Parameters.Add(parameter);
            }
            Cmd.Parameters.Add(
            new SqlParameter("ReturnValue", SqlDbType.Int, 4,
            ParameterDirection.ReturnValue, false, 0, 0,
            string.Empty, DataRowVersion.Default, null));

            return Cmd;
        }

        /// <summary> 
        /// 运行存储过程,返回. 
        /// </summary> 
        /// <param name="procName">存储过程名</param> 
        /// <param name="prams">存储过程参数</param> 
        /// <param name="dataReader">SqlDataReader对象</param> 
        public void RunProc(string procName, SqlParameter[] prams, SqlDataReader Dr)
        {

            SqlCommand Cmd = GetCommand(procName, prams, Dr);
            Dr = Cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return;
        }

        /// <summary> 
        /// 运行存储过程,返回. 
        /// </summary> 
        /// <param name="procName">存储过程名</param> 
        /// <param name="prams">存储过程参数</param> 
        public string RunProc(string procName, SqlParameter[] prams)
        {
            SqlDataReader Dr;
            SqlCommand Cmd = GetCommand(procName, prams);
            Dr = Cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            if (Dr.Read())
            {
                return Dr.GetValue(0).ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary> 
        /// 运行存储过程,返回dataset. 
        /// </summary> 
        /// <param name="procName">存储过程名.</param> 
        /// <param name="prams">存储过程入参数组.</param> 
        /// <returns>dataset对象.</returns> 
        public DataSet RunProc(string procName, SqlParameter[] prams, DataSet Ds)
        {
            SqlCommand Cmd = GetCommand(procName, prams);
            SqlDataAdapter Da = new SqlDataAdapter(Cmd);
            try
            {
                Da.Fill(Ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ds;
        }
    }
}
