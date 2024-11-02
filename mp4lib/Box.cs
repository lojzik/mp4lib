
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace JHa.MP4
{
    public class Box
    {
        protected SubStream Stream { get; }
        public long Size => Stream.Length;
        public String4 Type;
        public Guid? UserType { get; private set; }
        public Box(SubStream stream)
        {
            Stream = stream;
            ReadData();
        }
        protected virtual void ReadData()
        {
            Stream.Position = 0;
            Int64 size = ReadUInt32();
            Read(ref Type);
            if (size == 1)
            {
                size = (Int64)ReadUInt64();
            }
            else if (size == 0)
            {
                size = Stream.Length;
            }
            Stream.SetLength(size);
            if (Type == "uuid")
            {
                Span<byte> buffer = stackalloc byte[16];
                Stream.Read(buffer);
                UserType = new Guid(buffer);
            }
        }
        protected delegate T Converter<T>(ReadOnlySpan<byte> args);
        protected T Read<T>(Converter<T> converter) where T : struct
        {
            var size = Unsafe.SizeOf<T>();
            Span<byte> buff = stackalloc byte[size];
            Stream.Read(buff);
            if (BitConverter.IsLittleEndian)
                buff.Reverse();
            var result = converter(buff);
            return result;
        }
        protected UInt32 ReadUInt32() => Read(BitConverter.ToUInt32);
        protected Int32 ReadInt32() => Read(BitConverter.ToInt32);
        protected Int16 ReadInt16() => Read(BitConverter.ToInt16);
        protected UInt64 ReadUInt64() => Read(BitConverter.ToUInt64);
        protected Int64 ReadInt64() => Read(BitConverter.ToInt64);
        //        protected string ReadString4() => ReadString(4);
        protected string ReadString(int size)
        {
            Span<byte> buff = stackalloc byte[size];
            Stream.Read(buff);
            return Encoding.ASCII.GetString(buff);
        }
        protected void Read(ref String4 str)
        {
            unsafe
            {
                fixed (byte* p = str.ByteBuffer)
                {
                    var span = new Span<byte>(p, 4);
                    Stream.Read(span);
                }
            }
        }
        public override string ToString() => $"{Type} @{Stream.StartPosition}:{Stream.Length}:{UserType}";
        public static Box CreateBox(Stream stream, long startIndex)
        {
            var position = stream.Position;
            try
            {
                stream.Position = startIndex + 4;
                String4 xtype;
                xtype.Token = 0;
                unsafe
                {
                    var x = new Span<byte>(xtype.ByteBuffer, 4);
                    stream.Read(x);
                }
                var substream = new SubStream(stream, startIndex);
                switch (xtype.AsString)
                {
                    case "edts":
                    case "udta":
                    case "dinf": return new BoxNested(substream);
                    case "moov": return new BoxMOOV(substream);
                    case "trak": return new BoxTRAK(substream);
                    case "mdia": return new BoxMDIA(substream);
                    case "minf": return new BoxMINF(substream);
                    case "stbl": return new BoxSTBL(substream);
                    case "hdlr": return new BoxHDLR(substream);
                    case "ftyp": return new BoxFTYP(substream);
                    case "tkhd": return new BoxTKHD(substream);
                    case "stts": return new BoxSTTS(substream);
                    case "stsz": return new BoxSTSZ(substream);
                    case "stco": return new BoxSTCO(substream);
                    case "stss": return new BoxSTSS(substream);
                    case "stsc": return new BoxSTSC(substream);
                    case "stsd": return new BoxSTSD(substream);
                    case "elst": return new BoxELST(substream);
                    case "HMMT": return new BoxHMMT(substream);
                    case "co64":return new BoxCO64(substream);
                    default: return new Box(substream);
                }
            }
            finally
            {
                stream.Position = position;
            }
        }
    }
}
