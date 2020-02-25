using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ShadowGreatWall.Core.Reader
{
    abstract class EdianReader : IReader
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

        public double ReadDouble()
        {
            throw new NotImplementedException();
        }

        public short ReadInt16()
        {
            throw new NotImplementedException();
        }

        public int ReadInt32()
        {
            throw new NotImplementedException();
        }

        public long ReadInt64()
        {
            throw new NotImplementedException();
        }

        public sbyte ReadSByte()
        {
            throw new NotImplementedException();
        }

        public float ReadSingle()
        {
            throw new NotImplementedException();
        }

        public ushort ReadUInt16()
        {
            throw new NotImplementedException();
        }

        public uint ReadUInt32()
        {
            throw new NotImplementedException();
        }

        public ulong ReadUInt64()
        {
            throw new NotImplementedException();
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
    }
}
