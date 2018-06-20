using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer.Data.DapperEx.Commands
{
    /// <summary>
    /// 常用静态方法
    /// </summary>
    public static class Common
    {
        #region 静态属性

        /// <summary>
        /// 取日志记录器
        /// </summary>
        /// <returns></returns>
        //public static ILog Logger { get { return LogManager.GetLogger("LOG4ALL"); } }

        /// <summary>
        /// 取Appsetting节的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key] ?? string.Empty;
        }

        #endregion

        #region 　IO相关

        /// <summary>
        /// 取当前正在运行的程序集的绝对路径
        /// </summary>
        /// <returns></returns>
        public static string GetExecutingDirectory()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string assLocation = assembly.Location;
            return assLocation.Substring(0, assLocation.LastIndexOf('\\'));
        }

        #endregion

        #region 表达式树

        /// <summary>
        /// 返回真表达式
        /// </summary>
        public static Expression<Func<T, bool>> True<T>()
            where T : class
        {
            return f => true;
        }

        /// <summary>
        /// 返回假表达式
        /// </summary>
        public static Expression<Func<T, bool>> False<T>()
            where T : class
        {
            return f => false;
        }

        /// <summary>
        /// 拼接真表达式
        /// </summary>
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> TExp1,
            Expression<Func<T, bool>> TExp2) where T : class
        {
            var invokeExp = Expression.Invoke(TExp2, TExp1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(TExp1.Body, invokeExp), TExp1.Parameters);
        }

        /// <summary>
        /// 拼接假表达式
        /// </summary>
        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> TExp1,
            Expression<Func<T, bool>> TExp2) where T : class
        {
            var invokeExp = Expression.Invoke(TExp2, TExp1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(TExp1.Body, invokeExp), TExp1.Parameters);
        }

        #endregion

        #region 其它方法



        /// <summary>
        /// 执行指定委托，开放此方法的目的是简化try catch 块的写法
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="predicate">任务</param>
        /// <param name="parameter">参数</param>
        /// <param name="message">附带消息</param>
        /// <param name="msgArgs">附带消息参数</param>
        public static void Execute<T>(Action<T> predicate, T parameter, string message = "", params object[] msgArgs)
        {
            message = message ?? string.Empty;
            try
            {
                predicate.Invoke(parameter);
            }
            catch (Exception ex)
            {
                LogError(predicate, ex, message, msgArgs);
            }
        }

        /// <summary>
        /// 执行指定委托，开放此方法的目的是简化try catch 块的写法
        /// </summary>
        /// <typeparam name="T1">参数1类型</typeparam>
        /// <typeparam name="T2">参数2类型</typeparam>
        /// <param name="predicate">任务</param>
        /// <param name="parameter1">参数1</param>
        /// <param name="parameter2">参数2</param>
        /// <param name="message">附带消息</param>
        /// <param name="msgArgs">附带消息参数</param>
        public static void Execute<T1, T2>(Action<T1, T2> predicate, T1 parameter1, T2 parameter2, string message = "", params object[] msgArgs)
        {
            message = message ?? string.Empty;
            try
            {
                predicate.Invoke(parameter1, parameter2);
            }
            catch (Exception ex)
            {
                LogError(predicate, ex, message, msgArgs);
            }
        }

        /// <summary>
        /// 执行指定委托，开放此方法的目的是简化try catch 块的写法
        /// </summary>
        /// <typeparam name="T1">参数1类型</typeparam>
        /// <typeparam name="T2">参数2类型</typeparam>
        /// <typeparam name="T3">参数3类型</typeparam>
        /// <param name="predicate">任务</param>
        /// <param name="parameter1">参数1</param>
        /// <param name="parameter2">参数2</param>
        /// <param name="parameter3">参数3</param>
        /// <param name="message">附带消息</param>
        /// <param name="msgArgs">附带消息参数</param>
        public static void Execute<T1, T2, T3>(Action<T1, T2, T3> predicate, T1 parameter1, T2 parameter2, T3 parameter3, string message = "", params object[] msgArgs)
        {
            message = message ?? string.Empty;
            try
            {
                predicate.Invoke(parameter1, parameter2, parameter3);
            }
            catch (Exception ex)
            {
                LogError(predicate, ex, message, msgArgs);
            }
        }

        /// <summary>
        /// 执行指定委托，开放此方法的目的是简化try catch 块的写法
        /// </summary>
        /// <param name="predicate">任务</param>
        /// <param name="message">附带消息</param>
        /// <param name="msgArgs">附带消息参数</param>
        public static void Execute(Action predicate, string message = "", params object[] msgArgs)
        {
            message = message ?? string.Empty;
            try
            {
                predicate.Invoke();
            }
            catch (Exception ex)
            {
                LogError(predicate, ex, message, msgArgs);
            }
        }

        /// <summary>
        /// 执行指定委托，开放此方法的目的是简化try catch 块的写法
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="predicate">任务</param>
        /// <param name="message">附带消息</param>
        /// <param name="msgArgs">附带消息参数</param>
        /// <returns></returns>
        public static T Execute<T>(Func<T> predicate, string message = "", params object[] msgArgs)
        {
            T TValue = default(T);
            message = message ?? string.Empty;
            try
            {
                TValue = predicate.Invoke();
            }
            catch (Exception ex)
            {
                LogError(predicate, ex, message, msgArgs);
            }
            return TValue;
        }

        /// <summary>
        /// 执行指定委托，开放此方法的目的是简化try catch 块的写法
        /// </summary>
        /// <typeparam name="T1">参数1类型</typeparam>
        /// <typeparam name="T2">参数2类型</typeparam>
        /// <param name="predicate">任务</param>
        /// <param name="parameter">参数</param>
        /// <param name="message">附带消息</param>
        /// <param name="msgArgs">附带消息参数</param>
        /// <returns></returns>
        public static T2 Execute<T1, T2>(Func<T1, T2> predicate, T1 parameter, string message = "", params object[] msgArgs)
        {
            T2 TValue = default(T2);
            message = message ?? string.Empty;
            try
            {
                TValue = predicate.Invoke(parameter);
            }
            catch (Exception ex)
            {
                LogError(predicate, ex, message, msgArgs);
            }
            return TValue;
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="predicate">委托</param>
        /// <param name="ex">执行委托时的异常</param>
        /// <param name="message">出现异常时的附加消息</param>
        /// <param name="msgArgs">消息参数</param>
        /// <returns></returns>
        private static string LogError(Delegate predicate, Exception ex, string message = "", params object[] msgArgs)
        {
            string m_TypeFullName = predicate.Method.ReflectedType != null ? predicate.Method.ReflectedType.FullName : string.Empty;
            string methodName = predicate.Method != null ? predicate.Method.Name : string.Empty;
            methodName = string.Format("『{0}.{1}』", m_TypeFullName, methodName);

            if (string.IsNullOrEmpty(message)) message = "执行出现异常";
            message = string.Format(message, msgArgs);
            message = string.Format("{0}{1}{2}消息：{3}{4}{5}", methodName, message, Environment.NewLine, ex.Message, Environment.NewLine, ex.StackTrace);
            //Common.Logger.Error(message);

            return message;
        }

        // IDataRecord 转实体类变量
        private static readonly MethodInfo DataReader_Read = typeof(IDataReader).GetMethod("Read");
        private static readonly MethodInfo getValueMethod = typeof(IDataRecord).GetMethod("get_Item", new Type[] { typeof(int) });
        private static readonly MethodInfo isDBNullMethod = typeof(IDataRecord).GetMethod("IsDBNull", new Type[] { typeof(int) });

        /// <summary>
        /// 数据集转实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="table">数据源</param>
        /// <returns></returns>
        public static List<T> ToEntityList<T>(this DataTable table)
        {
            List<T> _EntityList = new List<T>();
            if (table == null || table.Rows.Count == 0) return _EntityList;
            DataTableReader dataRecord = table.CreateDataReader();

            DynamicMethod method = new DynamicMethod(string.Format("{0}.ToEntityList{1}", typeof(Common).FullName, Guid.NewGuid()),
                typeof(List<T>), new Type[] { typeof(IDataRecord) }, typeof(Common), true);
            ILGenerator il = method.GetILGenerator();

            LocalBuilder listBuilder = il.DeclareLocal(typeof(List<T>));
            LocalBuilder itemBuilder = il.DeclareLocal(typeof(T));
            System.Reflection.Emit.Label exitLabel = il.DefineLabel();
            System.Reflection.Emit.Label loopLabel = il.DefineLabel();

            //初始化List变量
            il.Emit(OpCodes.Newobj, typeof(List<T>).GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Stloc, listBuilder);

            il.MarkLabel(loopLabel);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Callvirt, DataReader_Read);
            il.Emit(OpCodes.Brfalse, exitLabel);

            //初始化空的T实例
            il.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Stloc_S, itemBuilder);

            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                PropertyInfo propertyInfo = typeof(T).GetProperty(dataRecord.GetName(i));
                System.Reflection.Emit.Label endIfLabel = il.DefineLabel();

                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Callvirt, isDBNullMethod);
                    il.Emit(OpCodes.Brtrue, endIfLabel);

                    il.Emit(OpCodes.Ldloc, itemBuilder);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Callvirt, getValueMethod);

                    Type propType = propertyInfo.PropertyType;
                    Type underType = Nullable.GetUnderlyingType(propType);
                    Type unboxType = underType != null ? underType : propType;

                    //if (propType.IsValueType)
                    //{
                    //    il.Emit(OpCodes.Unbox_Any, dataRecord.GetFieldType(i));
                    //}

                    if (unboxType == typeof(byte[]) || unboxType == typeof(string))
                    {
                        il.Emit(OpCodes.Castclass, propType);
                    }
                    else
                    {
                        il.Emit(OpCodes.Unbox_Any, dataRecord.GetFieldType(i));
                        if (underType != null) il.Emit(OpCodes.Newobj, propType.GetConstructor(new[] { underType }));
                    }

                    il.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                    il.MarkLabel(endIfLabel);
                }
            }


            il.Emit(OpCodes.Ldloc_S, listBuilder);
            il.Emit(OpCodes.Ldloc_S, itemBuilder);
            il.Emit(OpCodes.Callvirt, typeof(List<T>).GetMethod("Add"));

            il.Emit(OpCodes.Br, loopLabel);
            il.MarkLabel(exitLabel);

            il.Emit(OpCodes.Ldloc, listBuilder);
            il.Emit(OpCodes.Ret);

            Func<IDataRecord, List<T>> func = (Func<IDataRecord, List<T>>)method.CreateDelegate(typeof(Func<IDataRecord, List<T>>));
            _EntityList = func(dataRecord);

            return _EntityList;
        }

        #endregion
    }
}
