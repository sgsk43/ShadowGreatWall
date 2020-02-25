using System;
using System.Collections.Generic;
using System.Text;

namespace Org.Core.Log
{
    /// <summary>
    /// 日志信息
    /// </summary>
    [Serializable]
    internal class LogMsgInfo
    {
        /// <summary>
        /// 文件名称(带完整路径)
        /// </summary>
        public string FilePath = string.Empty;

        /// <summary>
        /// 日志消息
        /// </summary>
        public string Msg=string.Empty;

        /// <summary>
        /// 日志文件大小限制信息
        /// </summary>
        public SizeWithUnitInfo SizeUnit = new SizeWithUnitInfo();
    }
}
