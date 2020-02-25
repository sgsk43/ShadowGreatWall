using System;
using System.Text;
using System.Timers;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System.Text.RegularExpressions;

namespace Org.Core.Log
{
    /// <summary>
    /// 应用程序日志保存线程
    /// </summary>
    internal class AppLogSaveService
    {
        #region 属性变量
        private string defaultLogFile = string.Empty;
        private static readonly object _lockObj = new object();
        private string domain = string.Empty;
        private string key = string.Empty;
        private System.Timers.Timer t;
        private long bufferSize;
        #endregion

        #region 构造函数
        public AppLogSaveService()
        {
            this.bufferSize = 64 * 1024;  

            #region 启动定时清理计时器
            t = new System.Timers.Timer();

            t.Interval = 1000;
            t.Elapsed += new ElapsedEventHandler(SaveLogMsg); 
            #endregion
        }

        #endregion

        #region 获取当前DLL所在路径

        /// <summary>
        /// 获取当前DLL所在路径（以\\结尾,支持BS和CS）
        /// </summary>
        public static string AppPhysicalPath
        {
            get
            {
                //优先考虑WEB应用程序
                string path = HostingEnvironment.ApplicationPhysicalPath;

                if (path == null)
                {//其次考虑WINFORM应用程序
                    path = System.Threading.Thread.GetDomain().BaseDirectory;
                }

                if (!path.EndsWith("\\")) path = path + "\\";

                return path;
            }
        }
        #endregion

        #region 启动日志定时保存服务
        /// <summary>
        /// 启动日志定时保存服务
        /// </summary>
        public void StartService()
        {
            t.Start();
        }
        #endregion

        #region 保存日志信息
        private void SaveLogMsg(object sender, EventArgs e)
        {
            Dictionary<string,string> list = null;

            try
            {
                t.Stop();

                lock (_lockObj)
                {
                    //首先获取所有待保存日志,并且清空缓冲区
                    list = LogMsgHelper.Instance.GetAllLogMsgAndClean();
                }

                if (list != null && list.Count > 0)
                {
                    //循环保存每个日志信息
                    foreach (string file in list.Keys)
                    {
                        //保存信息到指定日志文件
                        WriteLog(file, list[file], ConvertToByteCountFromSizeAndUnit(LogFileHelper.Instance.GetLogFile(file)));
                    }
                }
            }
            catch (Exception)
            { 
                
            }
            finally
            {
                t.Start();
            }
        }
        #endregion

        #region 记录日志，内部方法
        /// <summary>
        /// 记录指定文件、指定大小的日志信息
        /// </summary>
        /// <param name="filePath">文件完整路劲</param>
        /// <param name="msg">日志</param>
        /// <param name="fileSize">日志文件大小</param>
        /// <returns></returns>
        private string WriteLog(string filePath, string msg, long fileSize)
        {
            lock (_lockObj)
            {
                try
                {
                    string fileIndex = string.Empty;
                    int maxIndex = 0;

                    //写入日志
                    WriteLogFile(filePath, msg);

                    //调用日志大小处理
                    if (GetFileLength(filePath) > fileSize)
                    {
                        //modify by zjoch 2010-6-21 如果大小达到限定，则重命名
                        DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(filePath));
                        FileInfo[] fis = di.GetFiles(Path.GetFileName(filePath) + "_*", SearchOption.TopDirectoryOnly);

                        if (fis.Length > 0)
                        {
                            for (int i = 0; i < fis.Length; i++)
                            {
                                try
                                {//找到最大的别名
                                    //MatchCollection mc = Regex.Matches(fis[i].Name, fis[i].Name + @"_\d+");
                                    string[] x = fis[i].Name.Split('_');
                                    fileIndex = x[x.Length-1];
                                    fileIndex = (fileIndex == string.Empty ? "0" : fileIndex);

                                    if (Convert.ToInt32(fileIndex) > maxIndex)
                                    {
                                        maxIndex = Convert.ToInt32(fileIndex);
                                    }
                                }
                                catch (Exception)
                                {//如果处理出现异常，就不考虑

                                }
                            }
                        }

                        new FileInfo(filePath).MoveTo(Path.GetDirectoryName(filePath) + @"\" + Path.GetFileName(filePath) + "_" + (maxIndex + 1));
                        

                        //ClearFile(filePath);
                    }
                }
                catch(Exception)
                {
                    
                }
                finally
                {

                }  
            }

            return msg;
        }
        #endregion

        #region 写日志文件
        private void WriteLogFile(string filePath, string msg)
        {
            //将字符串转换为字节
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);

            FileStream fs = null;

            if (filePath != null && filePath.Length > 0 && data != null && data.Length > 0)
            {
                try
                {
                    lock (_lockObj)
                    {

                        //确保文件存在 ，并且参数合法
                        if (!File.Exists(filePath))
                        {
                            this.CreateFile(filePath, 0);
                        }


                        fs = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.Write);

                        //确保起始位置在允许范围内
                        if (fs.Length >= 0)
                        {
                            WriteBuffer(fs, fs.Length, data.Length, data);
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                        fs = null;
                    }
                }
            }
        }

        private void WriteBuffer(FileStream fs, long startIndex, long length, byte[] data)
        {
            //写入字节数
            int bytesWrite = 0;

            if (length > data.Length)
            {
                length = data.Length;
            }
            else if (length < data.Length)
            {
                data = CutByteArray(data, 0, (int)length);
            }

            while (bytesWrite < data.Length)
            {
                fs.Position = startIndex + bytesWrite;

                if (data.Length - bytesWrite < bufferSize)
                {//当数据已经写完，但是还没到达指定结束边界，则结束
                    fs.Write(data, bytesWrite, data.Length - bytesWrite);
                    bytesWrite += data.Length - bytesWrite;
                }
                else
                {
                    fs.Write(data, bytesWrite, (int)bufferSize);
                    bytesWrite += (int)bufferSize;
                }
            }
        }

        private long GetFileLength(string filePath)
        {
            if (this.IsFileExist(filePath))
            {
                FileInfo fi = new FileInfo(filePath);

                if (fi != null)
                {
                    return fi.Length;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private bool IsFileExist(string filePath)
        {
            return File.Exists(Path.GetFullPath(filePath));
        }

        /// <summary>
        /// 清空文件内容
        /// </summary>
        /// <param name="filePath"></param>
        public void ClearFile(string filePath)
        {
            StreamWriter sw = null;

            if (filePath != null && filePath.Length > 0)
            {
                try
                {
                    lock (_lockObj)
                    {
                        //确保文件存在 ，并且参数合法
                        if (!IsFileExist(filePath))
                        {
                            this.CreateFile(filePath, 0);
                        }
                        else
                        {

                            sw = new StreamWriter(filePath, false);
                            sw.Write("");
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                        sw.Dispose();
                        sw = null;
                    }
                }
            }
        }

        private void CreateFile(string filePath,long fileSize)
        {
            FileStream fs = null;

            if (!IsFileExist(filePath))
            {
                try
                {
                    string directory = Path.GetDirectoryName(Path.GetFullPath(filePath));

                    if (!Directory.Exists(Path.GetFullPath(directory)))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    //由于调用Create方法后会返回一个文件流，这个流导致文件被占用，因此要释放
                    fs = File.Create(filePath);
                    fs.SetLength(fileSize);
                }
                catch (Exception)
                {

                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                        fs = null;
                    }
                }
            }
        }

        /// <summary>
        /// 截取字节数组
        /// </summary>
        /// <param name="input">待截取字节数组</param>
        /// <param name="offset">起始位置</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public byte[] CutByteArray(byte[] input, int offset, int length)
        {
            byte[] returnByteArray = new byte[length];

            for (int i = offset; i < length; i++)
            {
                returnByteArray[i] = input[i];
            }

            return returnByteArray;
        }

        /// <summary>
        /// 将带单位的大小转换为字节大小
        /// </summary>
        /// <param name="size">带单位大小</param>
        /// <param name="unit">单位</param>
        /// <returns></returns>
        public long ConvertToByteCountFromSizeAndUnit(SizeWithUnitInfo swui)
        {
            switch (swui.Unit)
            {
                case ByteUnit.B:
                    {
                        return (long)swui.Size;
                    }
                case ByteUnit.KB:
                    {
                        return (long)swui.Size * 1024;
                    }
                case ByteUnit.MB:
                    {
                        return (long)swui.Size * 1048576;
                    }
                case ByteUnit.GB:
                    {
                        return (long)swui.Size * 1073741824;
                    }
                default:
                    {
                        throw new Exception("不支持的单位换算!");
                    }
            }
        }
        #endregion
    }
}
