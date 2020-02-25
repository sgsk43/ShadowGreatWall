using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace Org.Core.Log
{
    /// <summary>
    /// 应用程序日志服务
    /// </summary>
    internal class AppLog : IAppLog
    {
        #region 属性变量
        private string defaultLogFile = string.Empty;

        private static readonly object _lockObj = new object();
        #endregion

        #region 构造函数
        public AppLog()
        {
            this.defaultLogFile = "AppRunLog.txt";
        }
        #endregion

        #region 拼接参数
        private string GetFullString(string[] msg)
        {
            string msgFull = string.Empty;

            //add by zjoch 2010-6-21 接收N个字符串参数，拼接
            for (int i = 0; i < msg.Length; i++)
            {
                msgFull += msg[i];
            }

            return msgFull;
        }
        #endregion

        #region 构建带时间毫秒数的日志
        /// <summary>
        /// 构建带时间毫秒数的日志
        /// </summary>
        /// <param name="msg">日志</param>
        public string BuildLogWithTime(params string[] msg)
        {
            return "[" + System.DateTime.Now.ToString() + ":" + System.DateTime.Now.Millisecond + "]->" + GetFullString(msg) + "\r\n";
        }
        #endregion

        #region 构建完整路径
        /// <summary>
        /// 构建完整路径
        /// </summary>
        /// <param name="fileName">文件名(不含路径信息)</param>
        /// <returns></returns>
        private string BuildFilePath(string fileName)
        {
            return Path.Combine(AppLogSaveService.AppPhysicalPath, @"App_Data\log\" + (fileName.ToLower().EndsWith(".txt") ? fileName : fileName + ".txt") );
        }
        #endregion

        #region 记录日志、日志文件名由类型决定
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">类型(从此产生日志文件名)</typeparam>
        /// <param name="msg">日志信息</param>
        /// <returns></returns>
        public string WriteLog<T>(params string[] msg) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), BuildLogWithTime(GetFullString(msg)));
        }

        public string WriteLog<T>(string msg) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), BuildLogWithTime(msg));
        }
        
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">类型(从此产生日志文件名)</typeparam>
        /// <param name="msg">日志信息</param>
        /// <returns></returns>
        public string WriteLog<T>(string msg,Exception ex) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), BuildLogWithTime(msg+"\r\n"+ex.ToString()));
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="msg">消息</param>
        /// <param name="su">日志大小</param>
        /// <returns></returns>
        public string WriteLog<T>(string msg, SizeWithUnitInfo su) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), BuildLogWithTime(msg), su);
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="msg">消息</param>
        /// <param name="su">日志大小</param>
        /// <returns></returns>
        public string WriteLog<T>(string msg,Exception ex, SizeWithUnitInfo su) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), BuildLogWithTime(msg+"\r\n"+ex.ToString()), su);
        }


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">类型(从此产生日志文件名)</typeparam>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        public string WriteLog<T>(Exception ex) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), BuildLogWithTime(ex.ToString()));
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        public string WriteLog<T>(Exception ex, SizeWithUnitInfo su) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), BuildLogWithTime(ex.ToString()), su);
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        public string WriteLog<T>(SizeWithUnitInfo su) where T : class
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(typeof(T).ToString()), string.Empty, su);
        }
        #endregion

        #region 记录日志、日志文件名由传入参数决定

        /// <summary>
        /// 记录日志(将记录在指定文件中)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="msg">日志内容</param>
        /// <returns></returns>
        public string WriteLog(string fileName, string msg)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(fileName), BuildLogWithTime(msg));
        }

        /// <summary>
        /// 记录日志(将记录在指定文件中)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="msg">日志内容</param>
        /// <param name="ex">异常对象</param>
        /// <returns></returns>
        public string WriteLog(string fileName, string msg,Exception ex)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(fileName), BuildLogWithTime(msg+"\r\n"+ex.ToString()));
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="msg">日志</param>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        public string WriteLog(string fileName, string msg, SizeWithUnitInfo su)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(fileName), BuildLogWithTime(msg), su);
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="msg">日志</param>
        /// <param name="ex">异常对象</param>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        public string WriteLog(string fileName, string msg,Exception ex, SizeWithUnitInfo su)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(fileName), BuildLogWithTime(msg+"\r\n"+ex.ToString()), su);
        }

        /// <summary>
        /// 记录日志(将记录在指定文件中)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        public string WriteLog(string fileName, Exception ex)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(fileName), BuildLogWithTime(ex.ToString()));
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="ex">异常</param>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        public string WriteLog(string fileName, Exception ex, SizeWithUnitInfo su)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(fileName), BuildLogWithTime(ex.ToString()), su);
        }

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        public string WriteLog(string fileName, SizeWithUnitInfo su)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(fileName),string.Empty, su);
        }

        #endregion

        #region 记录日志、日志文件名为默认
        /// <summary>
        /// 记录日志(将记录在默认文件AppRunLog.txt中)
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        public string WriteDefaultLog(string msg, Exception ex)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(this.defaultLogFile), BuildLogWithTime(msg + "\r\n"+ex.ToString()));
        }

        /// <summary>
        /// 记录日志(将记录在默认文件AppRunLog.txt中)
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <returns></returns>
        public string WriteDefaultLog(params string[] msg)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(this.defaultLogFile), BuildLogWithTime(GetFullString(msg)));
        }

        public string WriteDefaultLog(string msg)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(this.defaultLogFile), BuildLogWithTime(msg));
        }

        /// <summary>
        /// 记录日志(将记录在默认文件AppRunLog.txt中)
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <returns></returns>
        public string WriteDefaultLog(Exception ex)
        {
            return LogMsgHelper.Instance.AddLogMsg(BuildFilePath(this.defaultLogFile), BuildLogWithTime(ex.ToString()));
        }
        #endregion
    }
}