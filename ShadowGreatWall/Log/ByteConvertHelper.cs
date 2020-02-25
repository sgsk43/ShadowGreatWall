using System;
using System.Collections.Generic;
using System.Text;

namespace Org.Core.Log
{
    /// <summary>
    /// 字节转换帮助类
    /// </summary>
    public class ByteConvertHelper
    {
        #region 属性
        /// <summary>
        /// 单一实例
        /// </summary>
        public static readonly ByteConvertHelper Instance = new ByteConvertHelper();
        #endregion

        #region 方法

        #region 覆盖字节
        /// <summary>
        /// 覆盖字节(从左边索引0开始覆盖)
        /// </summary>
        /// <param name="source">被覆盖字节</param>
        /// <param name="direct">指定的字节来源</param>
        public void ReplaceByteArray(byte[] source, byte[] direct)
        {
            Array.Copy(direct, 0, source, 0, direct.Length);
        }


        /// <summary>
        /// 覆盖字节(从指定起始位置覆盖)
        /// </summary>
        /// <param name="source">被覆盖字节</param>
        /// <param name="direct">指定的字节来源</param>
        /// <param name="sourceOffset">被覆盖字节开始位置</param>
        public void ReplaceByteArray(byte[] source, byte[] direct, int sourceOffset)
        {
            Array.Copy(direct, 0, source, sourceOffset, direct.Length);
        }

        /// <summary>
        /// 覆盖字节(从指定起始位置覆盖指定长度)
        /// </summary>
        /// <param name="source">被覆盖字节</param>
        /// <param name="direct">指定的字节来源</param>
        /// <param name="sourceOffset">被覆盖字节开始位置</param>
        /// <param name="directOffset">指定的字节来源开始位置</param>
        /// <param name="ReplaceLength">置覆长度</param>
        public void ReplaceByteArray(byte[] source, byte[] direct, int sourceOffset, int directOffset, int ReplaceLength)
        {
            Array.Copy(direct, directOffset, source, sourceOffset, ReplaceLength);
        }
        #endregion

        #region 截取字节数组
        /// <summary>
        /// 截取字节数组
        /// </summary>
        /// <param name="input">待截取字节数组</param>
        /// <param name="offset">起始位置</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public byte[] CutByteArray(byte[] input,int offset,int length)
        {
            if (input != null && input.Length > 0 && offset >= 0 && length > 0)
            {
                byte[] returnByteArray = new byte[length];

                Array.Copy(input, offset, returnByteArray, 0, length);

                return returnByteArray;
            }
            else
            {
                return new byte[0];
            }
        }
        #endregion

        #region 反转字节数组
        /// <summary>
        /// 反转字节数组
        /// </summary>
        /// <param name="input">待反转字节数组</param>
        public byte[] ReverseByteArray(byte[] input)
        {
            if (input != null && input.Length > 0)
            {
                Array.Reverse(input);

                return input;
            }
            else
            {
                return new byte[0];
            }
        }
        #endregion
        
        #region 统计几个字节数组总长度
        /// <summary>
        /// 统计几个字节数组总长度
        /// </summary>
        /// <param name="byteArray">字节数组集合</param>
        /// <returns></returns>
        public long GetLenthOfByteArray(List<byte[]> byteArray)
        {
            int len = 0;

            if (byteArray != null && byteArray.Count > 0)
            {
                for (int i = 0; i < byteArray.Count; i++)
                {
                    len += byteArray[i].Length;
                }   
            }

            return len;
        }
        #endregion

        #region 合并几个字节数组

        /// <summary>
        /// 合并两个字节数组
        /// </summary>
        /// <param name="souce1">字节数组1</param>
        /// <param name="souce2">字节数组2</param>
        /// <returns></returns>
        public byte[] MergeByteArray(byte[] souce1, byte[] souce2)
        {
            if (souce1 != null && souce2 != null)
            {
                List<byte[]> byteArray = new List<byte[]>();

                byteArray.Add(souce1);
                byteArray.Add(souce2);

                return MergeByteArray(byteArray);
            }
            else
            {
                return new byte[0];
            }
        }

        /// <summary>
        /// 合并几个字节数组
        /// </summary>
        /// <param name="byteArray">字节数组集合</param>
        /// <returns></returns>
        public byte[] MergeByteArray(List<byte[]> byteArray)
        {
            if (byteArray != null && byteArray.Count > 0)
            {
                int mergedLength = 0;

                byte[] direct = new byte[GetLenthOfByteArray(byteArray)]; ;

                for (int i = 0; i < byteArray.Count; i++)
                {
                    Array.Copy(byteArray[i], 0, direct, mergedLength, byteArray[i].Length);

                    mergedLength += byteArray[i].Length;
                }

                return direct;
            }
            else
            {
                return new byte[0];
            }
        }
        #endregion

        #region 转换为带单位大小的字符串(例如：42M、2.3G)
        /// <summary>
        /// 转换为带单位大小的字符串(例如：42M、2.3G)
        /// </summary>
        /// <param name="byteCount">字节数</param>
        /// <returns></returns>
        public string ConvertToSizeUnitStringFromByteCount(long byteCount)
        {
            SizeWithUnitInfo swui = ConvertToSizeWithUnitInfoFromByteCount(byteCount);

            return swui.Size + " " + GetUnitName(swui.Unit);
        }
        #endregion

        #region 转换为带单位大小实体
        /// <summary>
        /// 转换为带单位大小实体
        /// </summary>
        /// <param name="byteCount">字节数</param>
        /// <returns></returns>
        public SizeWithUnitInfo ConvertToSizeWithUnitInfoFromByteCount(long byteCount)
        {
            SizeWithUnitInfo swui = new SizeWithUnitInfo();

            swui.Bytes = byteCount;

            if (byteCount >= 1073741824)
            {
                swui.Size = double.Parse(String.Format("{0:##.##}", byteCount / 1073741824));
                swui.Unit = ByteUnit.GB;
            }
            else if (byteCount >= 1048576)
            {
                swui.Size = double.Parse(String.Format("{0:##.##}", byteCount / 1048576));
                swui.Unit = ByteUnit.MB;
            }
            else if (byteCount >= 1024)
            {
                swui.Size = double.Parse(String.Format("{0:##.##}", byteCount / 1024));
                swui.Unit = ByteUnit.KB;
            }
            else if (byteCount > 0 && byteCount < 1024)
            {
                swui.Size = byteCount;
                swui.Unit = ByteUnit.B;
            }

            return swui;
        }
        #endregion

        #region 将带单位的大小转换为字节大小
        /// <summary>
        /// 将带单位的大小转换为字节大小
        /// </summary>
        /// <param name="swui">带单位的大小信息</param>
        /// <returns></returns>
        public long ConvertToByteCountFromSizeAndUnit(SizeWithUnitInfo swui)
        {
            if (swui != null)
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
            else
            {
                return 0;
            }
        }
        #endregion

        #region 将字节大小、单位转换为带单位的大小
        /// <summary>
        /// 换算单位
        /// </summary>
        /// <param name="byteCount">大小</param>
        /// <param name="by">单位</param>
        /// <returns></returns>
        public SizeWithUnitInfo ConvertToSizeWithUnitInfoFromByteCountAndUnit(long byteCount, ByteUnit by)
        {
            SizeWithUnitInfo swui = new SizeWithUnitInfo();

            swui.Bytes = byteCount;

            switch (by)
            {
                case ByteUnit.B:
                    {
                        swui.Size = byteCount;
                        swui.Unit = ByteUnit.B;
                        break;
                    }
                case ByteUnit.KB:
                    {
                        swui.Size = double.Parse(String.Format("{0:##.##}", byteCount / 1024));
                        swui.Unit = ByteUnit.KB;
                        break;
                    }
                case ByteUnit.MB:
                    {
                        swui.Size = double.Parse(String.Format("{0:##.##}", byteCount / 1048576));
                        swui.Unit = ByteUnit.MB;
                        break;
                    }
                case ByteUnit.GB:
                    {
                        swui.Size = double.Parse(String.Format("{0:##.##}", byteCount / 1073741824));
                        swui.Unit = ByteUnit.GB;
                        break;
                    }
                default:
                    {
                        throw new Exception("不支持的单位换算!");
                    }
            }


            return swui;
        }
        #endregion

        #region 根据字节数获取字节单位
        /// <summary>
        /// 根据字节数获取字节单位
        /// </summary>
        /// <param name="byteCount">字节数</param>
        /// <returns></returns>
        public ByteUnit ConvertToUnitFromByteCount(long byteCount)
        {
            ByteUnit size = ByteUnit.B;

            if (byteCount >= 1073741824)
                size = ByteUnit.GB;
            else if (byteCount >= 1048576)
                size = ByteUnit.MB;
            else if (byteCount >= 1024)
                size = ByteUnit.KB;
            else if (byteCount > 0 && byteCount < 1024)
                size = ByteUnit.B;

            return size;
        }
        #endregion

        #region 根据字节单位获取对应名称
        /// <summary>
        /// 根据字节单位获取对应名称
        /// </summary>
        /// <param name="unit">单位</param>
        /// <returns></returns>
        public string GetUnitName(ByteUnit unit)
        {
            switch (unit)
            {
                case ByteUnit.B:
                    {
                        return "B";
                    }

                case ByteUnit.GB:
                    {
                        return "GB";
                    }

                case ByteUnit.KB:
                    {
                        return "KB";
                    }

                case ByteUnit.MB:
                    {
                        return "MB";
                    }
                default:
                    {
                        throw new Exception("未知单位!");
                    }
            }
        }
        #endregion

        #endregion
    }

    #region 字节单位
    /// <summary>
    /// 字节单位
    /// </summary>
    public enum ByteUnit
    {
        /// <summary>
        /// G字节
        /// </summary>
        GB,

        /// <summary>
        /// 兆字节
        /// </summary>
        MB,

        /// <summary>
        /// 千字节
        /// </summary>
        KB,

        /// <summary>
        /// 字节
        /// </summary>
        B,
    }
    #endregion

    #region 带单位的大小信息
    /// <summary>
    /// 带单位的大小信息
    /// </summary>
    [Serializable]
    public class SizeWithUnitInfo
    {
        private long _bytes;

        /// <summary>
        /// 换算后大小(如果没有指定Bytes的值，则按Size和Unit进行换算，此换算会丢失精度)
        /// </summary>
        public double Size;

        /// <summary>
        /// 字节数(用于计算，防止换算过程中数据失真)
        /// </summary>
        public long Bytes
        {
            get { return ByteConvertHelper.Instance.ConvertToByteCountFromSizeAndUnit(this); }
            set { this._bytes = value; }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public ByteUnit Unit;
    }
    #endregion
}
