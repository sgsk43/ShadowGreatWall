using System;
using System.Text;
using System.Collections.Generic;

namespace Org.Core.Log
{
    /// <summary>
    /// 日志文件帮助类
    /// </summary>
    internal class LogFileHelper
    {
        #region 属性变量
        public static readonly LogFileHelper Instance = new LogFileHelper();
        public static Dictionary<string, SizeWithUnitInfo> LogFileList;
        private SizeWithUnitInfo sizeUnit = null;
        #endregion

        #region 方法

        #region 构造函数
        public LogFileHelper()
        {
            LogFileList = new Dictionary<string, SizeWithUnitInfo>();

            //默认日志文件为1M
            this.sizeUnit = new SizeWithUnitInfo();
            this.sizeUnit.Size = 5;
            this.sizeUnit.Unit = ByteUnit.MB;
        }
        #endregion

        #region 获取日志文件信息
        /// <summary>
        /// 获取日志文件信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>获取日志文件信息</returns>
        public SizeWithUnitInfo GetLogFile(string filePath)
        {
            if (LogFileList != null && LogFileList.Count > 0)
            {
                if (LogFileList.ContainsKey(filePath))
                {
                    return LogFileList[filePath];
                }
                else
                {
                    return this.sizeUnit;
                }
            }
            else
            {
                return this.sizeUnit;
            }
        }
        #endregion

        #region 添加日志文件信息
        /// <summary>
        /// 添加日志文件信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="su">带单位的大小信息</param>
        public void AddLogFile(string filePath,SizeWithUnitInfo su)
        {
            if (LogFileList != null && LogFileList.Count > 0)
            {
                if (LogFileList.ContainsKey(filePath))
                {
                    LogFileList[filePath] = su;
                }
                else
                {
                    LogFileList.Add(filePath, su);
                }
            }
            else
            {
                LogFileList.Add(filePath, su);
            }
        }
        #endregion

        #endregion
    }
}
