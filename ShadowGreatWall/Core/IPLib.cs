using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ShadowGreatWall.Core
{
    static class IPLib
    {
        #region [变量]
        private static BinaryReader reader = null;
        private static int firstOffset = 0;
        private static int lastOffset = 0;
        private static int total = 0;
        #endregion

        #region [初始化]
        static IPLib()
        {
            Init();
        }

        private static void Init()
        {
            reader = new BinaryReader(new MemoryStream(Properties.Resources.qqwry));

            firstOffset = reader.ReadInt32();
            lastOffset = reader.ReadInt32();
            total = (lastOffset - firstOffset) / 7;
        }
        #endregion

        #region [接口]
        public static string GetNation(string ip)
        {
            Regex regex = new Regex(@"^((?=\b|\D)((\d{1,2}|1\d{1,2}|2[0-4]\d|25[0-5])\.){3}(\d{1,2}|1\d{1,2}|2[0-4]\d|25[0-5])(?=\b|\D))$");

            if (!regex.IsMatch(ip))
            {
                return null;
            }

            var rawIP = GetRawIP(ip);
            int lower = 0;
            int upper = total;
            int find = lastOffset;

            while (lower <= upper)
            { 
                int i = (int)Math.Floor((lower + upper) / 2.0);
                reader.BaseStream.Position = firstOffset + i * 7;

                //string beginIP = new string(
            }
        }
        #endregion

        #region [函数]
        private static uint GetRawIP(string ip)
        {
            var ipArr = ip.Split('.');

            return (16777216U * Convert.ToUInt32(ipArr[0])) + (65536U * Convert.ToUInt32(ipArr[1])) + (256U * Convert.ToUInt32(ipArr[2])) + Convert.ToUInt32(ipArr[3]);
        }
        #endregion
    }
}
