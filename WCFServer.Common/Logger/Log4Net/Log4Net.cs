using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServer.Common.Log4Net
{

    /// <summary>
    /// Logger 若使用Log4Net请先初始化Loger4Net【InitLogger()】
    /// </summary>
    public class Log4Net
    {
        private Log4Net() { }

        #region log4net

        private static readonly ConcurrentDictionary<Type, ILog> Loggers = new ConcurrentDictionary<Type, ILog>();

        #region 初始化Loger4Net
        /// <summary>
        /// 初始化Loger4Net 请确认log4net.config文件是否正确
        /// </summary>
        public static void InitLogger()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
        }
        #endregion

        #region 获取记录器
        /// <summary>
        /// 获取记录器
        /// </summary>
        /// <param name="source">soruce</param>
        /// <returns>ILog</returns>
        private static ILog GetLogger(Type source)
        {
            if (Loggers.ContainsKey(source))
            {
                return Loggers[source];
            }
            else
            {
                ILog logger = LogManager.GetLogger(source);
                Loggers.TryAdd(source, logger);
                return logger;
            }
        }
        #endregion

        /* Log a message object */
        #region debug
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Debug(object source, string message)
        {
            Debug(source.GetType(), message);
        }

        /// <summary>
        /// 调试信息 带stringFromat
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="ps">ps</param>
        public static void Debug(object source, string message, params object[] ps)
        {
            Debug(source.GetType(), string.Format(message, ps));
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Debug(Type source, string message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsDebugEnabled)
            {
                logger.Debug(message);
            }
        }
        #endregion

        #region info

        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Info(object source, string message)
        {
            Info(source.GetType(), message);
        }

        /// <summary>
        /// 关键信息 带stringFromat
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="ps">ps</param>
        public static void Info(object source, string message, params object[] ps)
        {
            Info(source.GetType(), string.Format(message, ps));
        }


        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="console">是否打印到控制台,默认false</param>
        public static void Info(Type source, string message, bool console = false)
        {
            ILog logger = GetLogger(source);
            if (logger.IsInfoEnabled)
            {
                logger.Info(message);
            }
            if (console)
            {
                Console.WriteLine(message);
            }

        }
        #endregion

        #region warn
        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Warn(object source, object message)
        {
            Warn(source.GetType(), message);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Warn(Type source, object message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsWarnEnabled)
            {
                logger.Warn(message);
            }
        }

        #endregion

        #region error
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Error(object source, object message)
        {
            Error(source.GetType(), message);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Error(Type source, object message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsErrorEnabled)
            {
                logger.Error(message);
            }
        }
        #endregion

        #region fatal
        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Fatal(object source, object message)
        {
            Fatal(source.GetType(), message);
        }

        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        public static void Fatal(Type source, object message)
        {
            ILog logger = GetLogger(source);
            if (logger.IsFatalEnabled)
            {
                logger.Fatal(message);
            }
        }
        #endregion


        /* Log a message object and exception */

        #region debug
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Debug(object source, object message, Exception exception)
        {
            Debug(source.GetType(), message, exception);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Debug(Type source, object message, Exception exception)
        {
            GetLogger(source).Debug(message, exception);
        }
        #endregion

        #region info
        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Info(object source, object message, Exception exception)
        {
            Info(source.GetType(), message, exception);
        }

        /// <summary>
        /// 关键信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Info(Type source, object message, Exception exception)
        {
            GetLogger(source).Info(message, exception);
        }
        #endregion

        #region warn
        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Warn(object source, object message, Exception exception)
        {
            Warn(source.GetType(), message, exception);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Warn(Type source, object message, Exception exception)
        {
            GetLogger(source).Warn(message, exception);
        }
        #endregion

        #region error
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Error(object source, object message, Exception exception)
        {
            Error(source.GetType(), message, exception);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Error(Type source, object message, Exception exception)
        {
            GetLogger(source).Error(message, exception);
        }
        #endregion

        #region fatal
        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Fatal(object source, object message, Exception exception)
        {
            Fatal(source.GetType(), message, exception);
        }

        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="message">message</param>
        /// <param name="exception">ex</param>
        public static void Fatal(Type source, object message, Exception exception)
        {
            GetLogger(source).Fatal(message, exception);
        }
        #endregion

        #endregion
    }
}
