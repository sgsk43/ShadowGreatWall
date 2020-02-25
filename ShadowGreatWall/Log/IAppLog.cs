using System;
using System.IO;
using System.Text;

namespace Org.Core.Log
{
    /// <summary>
    /// 应用程序日志服务接口
    /// </summary>
    public interface IAppLog
    {
        #region 构建带时间毫秒数的日志
        /// <summary>
        /// 构建带时间毫秒数的日志
        /// </summary>
        /// <param name="msg">日志</param>
        string BuildLogWithTime(params string[] msg);
        #endregion

        #region 记录日志、日志文件名由类型决定

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">类型(从此产生日志文件名)</typeparam>
        /// <param name="msg">日志信息</param>
        /// <returns></returns>
        string WriteLog<T>(string msg) where T : class;

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">类型(从此产生日志文件名)</typeparam>
        /// <param name="msg">日志信息</param>
        /// <returns></returns>
        string WriteLog<T>(params string[] msg) where T : class;

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">类型(从此产生日志文件名)</typeparam>
        /// <param name="msg">日志信息</param>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        string WriteLog<T>(string msg,Exception ex) where T : class;


        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="msg">消息</param>
        /// <param name="su">日志大小</param>
        /// <returns></returns>
        string WriteLog<T>(string msg, SizeWithUnitInfo su) where T : class;

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        /// <param name="su">日志大小</param>
        /// <returns></returns>
        string WriteLog<T>(string msg,Exception ex, SizeWithUnitInfo su) where T : class;

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">类型(从此产生日志文件名)</typeparam>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        string WriteLog<T>(Exception ex) where T : class;

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        string WriteLog<T>(Exception ex, SizeWithUnitInfo su) where T : class;

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        string WriteLog<T>(SizeWithUnitInfo su) where T : class;
        #endregion

        #region 记录日志、日志文件名由传入参数决定

        /// <summary>
        /// 记录日志(将记录在指定文件中)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="msg">日志内容</param>
        /// <returns></returns>
        string WriteLog(string fileName, string msg);

        /// <summary>
        /// 记录日志(将记录在指定文件中)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="msg">日志内容</param>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        string WriteLog(string fileName, string msg,Exception ex);

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="msg">日志</param>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        string WriteLog(string fileName, string msg, SizeWithUnitInfo su);

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="msg">日志</param>
        /// <param name="ex">异常</param>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        string WriteLog(string fileName, string msg,Exception ex, SizeWithUnitInfo su);

        /// <summary>
        /// 记录日志(将记录在指定文件中)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        string WriteLog(string fileName, Exception ex);

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="ex">异常</param>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        string WriteLog(string fileName, Exception ex, SizeWithUnitInfo su);

        /// <summary>
        /// 记录日志(带文件大小限制)
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="su">文件大小限制</param>
        /// <returns></returns>
        string WriteLog(string fileName, SizeWithUnitInfo su);

        #endregion

        #region 记录日志、日志文件名为默认
        /// <summary>
        /// 记录日志(将记录在默认文件AppRunLog.txt中)
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        string WriteDefaultLog(string msg, Exception ex);

        /// <summary>
        /// 记录日志(将记录在默认文件AppRunLog.txt中)
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <returns></returns>
        string WriteDefaultLog(params string[] msg);

        /// <summary>
        /// 记录日志(将记录在默认文件AppRunLog.txt中)
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <returns></returns>
        string WriteDefaultLog(string msg);

        /// <summary>
        /// 记录日志(将记录在默认文件AppRunLog.txt中)
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <returns></returns>
        string WriteDefaultLog(Exception ex);
        #endregion
    }
}