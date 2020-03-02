using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace ShadowGreatWall.Core.Reader
{
    public class BinaryReader : IReader, IDisposable
    {
        #region [变量]
        private Stream baseStream = null;
        private Encoding baseEncoding = Encoding.Default;

        private IReader bigEdianReader = null;
        private IReader littleEdianReader = null;
        #endregion

        #region [初始化]
        public BinaryReader(Stream input) : this(input, Encoding.Default)
        {
            
        }

        public BinaryReader(Stream input, Encoding encoding)
        {
            this.baseStream = input;
            this.baseEncoding = encoding;

            this.littleEdianReader = new EdianReader(this.baseStream, true);
            this.bigEdianReader = new EdianReader(this.baseStream, false);
        }
        #endregion

        #region [属性]
        public virtual Stream BaseStream 
        {
            get
            {
                return baseStream;
            }
        }

        public IReader DefaultReader
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                {
                    return littleEdianReader;
                }
                else
                {
                    return bigEdianReader;
                }
            }
        }

        public IReader LittleEdianReader
        {
            get
            {
                return this.littleEdianReader;
            }
        }

        public IReader BigEdianReader
        {
            get
            {
                return this.bigEdianReader;
            }
        }
        #endregion

        #region [析构]
        public virtual void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (this.baseStream != null)
            {
                this.baseStream.Close();
                this.baseStream = null;
            }
        }
        #endregion

        public int PeekChar()
        {
            return DefaultReader.PeekChar();
        }

        public int Read()
        {
            return DefaultReader.Read();
        }

        public int Read(byte[] buffer, int index, int count)
        {
            return DefaultReader.Read(buffer, index, count);
        }

        public int Read(char[] buffer, int index, int count)
        {
            return DefaultReader.Read(buffer, index, count);
        }

        public bool ReadBoolean()
        {
            return DefaultReader.ReadBoolean();
        }

        public byte ReadByte()
        {
            return DefaultReader.ReadByte();
        }

        public byte[] ReadBytes(int count)
        {
            return DefaultReader.ReadBytes(count);
        }

        public char ReadChar()
        {
            return DefaultReader.ReadChar();
        }

        public char[] ReadChars(int count)
        {
            return DefaultReader.ReadChars(count);
        }

        public decimal ReadDecimal()
        {
            return DefaultReader.ReadDecimal();
        }

        public double ReadDouble()
        {
            return DefaultReader.ReadDouble();
        }

        public short ReadInt16()
        {
            return DefaultReader.ReadInt16();
        }

        public int ReadInt32()
        {
            return DefaultReader.ReadInt32();
        }

        public long ReadInt64()
        {
            return DefaultReader.ReadInt64();
        }

        public sbyte ReadSByte()
        {
            return DefaultReader.ReadSByte();
        }

        public float ReadSingle()
        {
            return DefaultReader.ReadSingle();
        }

        public ushort ReadUInt16()
        {
            return DefaultReader.ReadUInt16();
        }

        public uint ReadUInt32()
        {
            return DefaultReader.ReadUInt32();
        }

        public ulong ReadUInt64()
        {
            return DefaultReader.ReadUInt64();
        }


        public string ReadString()
        {
            return DefaultReader.ReadString();
        }
    }
}
