using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShadowGreatWall.Core.Reader
{
    interface IReader
    {
        int PeekChar();
        int Read();
        public virtual int Read(byte[] buffer, int index, int count);
        public virtual int Read(char[] buffer, int index, int count);
        public virtual bool ReadBoolean();
        public virtual byte ReadByte();
        public virtual byte[] ReadBytes(int count);
        public virtual char ReadChar();
        public virtual char[] ReadChars(int count);
        public virtual decimal ReadDecimal();
        public virtual double ReadDouble();
        public virtual short ReadInt16();
        public virtual int ReadInt32();
        public virtual long ReadInt64();
        public virtual sbyte ReadSByte();
        public virtual float ReadSingle();
        public virtual ushort ReadUInt16();
        public virtual uint ReadUInt32();
        public virtual ulong ReadUInt64();
    }
}
