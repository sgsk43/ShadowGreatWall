using System;
using System.Threading;

namespace Org.Core.Log
{
    /// <summary>
    /// 应用程序日志服务代理
    /// </summary>
    public class AppLogProxy
    {
        #region 属性变量
        private static Thread threadForAppLogSaveService;
        private static readonly IAppLog instance_AppLog;
        private static readonly AppLogSaveService alss;

        /// <summary>
        /// 获取件系统IO服务代理(该日志服务独立于其他服务，因此所有调用都是内部独立的)
        /// </summary>
        public static IAppLog AppLog
        {
            get { return instance_AppLog; }
        }

        /// <summary>
        /// 字节转换帮助
        /// </summary>
        public static ByteConvertHelper ByteConvert
        {
            get { return ByteConvertHelper.Instance; }
        }

        /// <summary>
        /// 当前系统运行路径(支持BS、CS程序)
        /// </summary>
        public static string AppPhysicalPath
        {
            get { return AppLogSaveService.AppPhysicalPath; }
        }

        #endregion

        #region 构造方法

        static AppLogProxy()
        {
            instance_AppLog = new AppLog();

            //启动日志定时保存服务
            alss = new AppLogSaveService();
            ThreadStart threadStartForAppLogSaveService = new ThreadStart(alss.StartService);
            threadForAppLogSaveService = new Thread(threadStartForAppLogSaveService);
            threadForAppLogSaveService.IsBackground = true;
            threadForAppLogSaveService.Start();
        }

        #endregion
    }
}