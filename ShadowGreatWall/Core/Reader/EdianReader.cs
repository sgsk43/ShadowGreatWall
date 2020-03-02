using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ShadowGreatWall.Core.Reader
{
    class EdianReader : IReader
    {
        private Stream baseStream = null;
        private bool needToReverse = false;

        public EdianReader(Stream stream, bool isLittleEdian)
        {
            this.baseStream = stream;
            this.needToReverse = !isLittleEdian;
        }

        public int PeekChar()
        {
            long position = this.baseStream.Position;
            int ret = this.Read();

            this.baseStream.Position = position;

            return ret;
        }

        public int Read()
        {
            return this.baseStream.ReadByte();
        }

        public int Read(byte[] buffer, int index, int count)
        {
            return this.baseStream.Read(buffer, index, count);
        }

        public int Read(char[] buffer, int index, int count)
        {
            byte[] byteBuffer = new byte[buffer.Length];

            int ret = this.Read(byteBuffer, index, count);

            for (int i = 0; i < byteBuffer.Length; i++)
            {
                buffer[i] = Convert.ToChar(byteBuffer[i]);
            }

            return ret;
        }

        public bool ReadBoolean()
        {
            return this.Read() != 0;
        }

        public byte ReadByte()
        {
            return (byte)this.Read();
        }

        public byte[] ReadBytes(int count)
        {
            byte[] buffer = new byte[count];

            this.Read(buffer, 0, count);

            return buffer;
        }

        public char ReadChar()
        {
            return Convert.ToChar(this.ReadByte());
        }

        public char[] ReadChars(int count)
        {
            char[] buffer = new char[count];

            this.Read(buffer, 0, count);

            return buffer;
        }

        public decimal ReadDecimal()
        {
            return (decimal)typeof(Decimal).GetMethod("ToDecimal", new Type[] { typeof(byte[]) }).Invoke(null, new object[] { ReadBuffer(0x10) });
        }

        public unsafe double ReadDouble()
        {
            byte[] buffer = ReadBuffer(8);
            uint num = (uint)(((buffer[0] | (buffer[1] << 8)) | (buffer[2] << 0x10)) | (buffer[3] << 0x18));
            uint num2 = (uint)(((buffer[4] | (buffer[5] << 8)) | (buffer[6] << 0x10)) | (buffer[7] << 0x18));
            ulong num3 = (num2 << 0x20) | num;
            return *(((double*)&num3));
        }


        public short ReadInt16()
        {
            byte[] buffer = ReadBuffer(2);
            return (short)(buffer[0] | (buffer[1] << 8));
        }

        public int ReadInt32()
        {
            byte[] buffer = ReadBuffer(4);
            return (((buffer[0] | (buffer[1] << 8)) | (buffer[2] << 0x10)) | (buffer[3] << 0x18));
        }

        public long ReadInt64()
        {
            byte[] buffer = ReadBuffer(8);

            uint num = (uint)(((buffer[0] | (buffer[1] << 8)) | (buffer[2] << 0x10)) | (buffer[3] << 0x18));
            uint num2 = (uint)(((buffer[4] | (buffer[5] << 8)) | (buffer[6] << 0x10)) | (buffer[7] << 0x18));

            return (long)((num2 << 0x20) | num);
        }


        public sbyte ReadSByte()
        {
            byte[] buffer = ReadBuffer(1);
            return (sbyte)buffer[0];
        }

        public unsafe float ReadSingle()
        {
            byte[] buffer = ReadBuffer(4);
            uint num = (uint)(((buffer[0] | (buffer[1] << 8)) | (buffer[2] << 0x10)) | (buffer[3] << 0x18));
            return *(((float*)&num));

        }

        public ushort ReadUInt16()
        {
            byte[] buffer = ReadBuffer(2);
            return (ushort)(buffer[0] | (buffer[1] << 8));
        }

        public uint ReadUInt32()
        {
            byte[] buffer = ReadBuffer(4);
            return (uint)(((buffer[0] | (buffer[1] << 8)) | (buffer[2] << 0x10)) | (buffer[3] << 0x18));
        }

        public ulong ReadUInt64()
        {
            byte[] buffer = ReadBuffer(8);
            uint num = (uint)(((buffer[0] | (buffer[1] << 8)) | (buffer[2] << 0x10)) | (buffer[3] << 0x18));
            uint num2 = (uint)(((buffer[4] | (buffer[5] << 8)) | (buffer[6] << 0x10)) | (buffer[7] << 0x18));
            return ((num2 << 0x20) | num);
        }

        private byte[] ReadBuffer(int count)
        {
            byte[] ret = this.ReadBytes(count);

            if (needToReverse)
            {
                Array.Reverse(ret);
            }

            return ret;
        }


        public string ReadString()
        {
            StringBuilder builder = new StringBuilder();
            
            char data;
            while ((data = this.ReadChar()) != 0)
            {
                builder.Append(data);
            }

            return builder.ToString();
        }
    }
}
