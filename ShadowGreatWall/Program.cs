using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Security.Cryptography;
using System.Text;
using Org.Core.Log;
using Microsoft.Win32;
using System.IO;
using ShadowGreatWall.Core;
using ShadowGreatWall.Startup;
using ShadowGreatWall.Encryption;

namespace ShadowGreatWall
{
    static class Program
    {
        private static readonly string mutexKey;

        static Program()
        {
            mutexKey = "ShadowGrateWall_" + Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.Default.GetBytes(Application.StartupPath)));
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(false, mutexKey))
            {
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
                Application.EnableVisualStyles();
                Application.ApplicationExit += OnApplicationExit;
                //SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
                Application.SetCompatibleTextRenderingDefault(false);

                if (!mutex.WaitOne(0, false))
                {
                    return;
                }

                Directory.SetCurrentDirectory(Application.StartupPath);

                EncryptionCenter.Init();
                StartupMgr.Instance.Start();

                Application.Run(new frmMain());
            }
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            
        }

        private static int exited = 0;

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (Interlocked.Increment(ref exited) == 1)
            {
                if (e.ExceptionObject != null)
                {
                    AppLogProxy.AppLog.WriteLog("异常日志", e.ExceptionObject.ToString());
                }

                MessageBox.Show("系统出现未捕获的异常，请查看日志", "系统即将退出", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Application.Exit();
            }
        }
    }
}
