
using System;
using System.IO;
using System.Text;

namespace JHa.MP4
{
    public class Box
    {
        protected Stream Stream { get; }
        protected long StartIndex { get; }
        public long Size { get; private set; }
        public string Type { get; private set; }
        public Guid? UserType { get; private set; }
        public Box(Stream stream, long startIndex)
        {
            Stream = stream;
            StartIndex = startIndex;
            var position = stream.Position;
            try
            {
                ReadData();
            }
            finally
            {
                stream.Position = position;
            }
        }
        private static byte[] buffer2 = new byte[2];
        private static byte[] buffer4 = new byte[4];
        private static byte[] buffer8 = new byte[8];
        protected virtual void ReadData()
        {
            Stream.Position = StartIndex;
            Size = ReadUInt32();
            Type = ReadString4();
            if (Size == 1)
            {
                Size = (Int64)ReadUInt64();
            }
            else if (Size == 0)
            {
                Size = Stream.Length - StartIndex;
            }
            if (Type == "uuid")
            {
                var buffer = new byte[16];
                Stream.Read(buffer, 0, 16);
                UserType = new Guid(buffer);
            }
        }
        protected UInt32 ReadUInt32()
        {
            Stream.Read(buffer4, 0, 4);
            Array.Reverse(buffer4);
            return BitConverter.ToUInt32(buffer4, 0);
        }
        protected Int32 ReadInt32()
        {
            Stream.Read(buffer4, 0, 4);
            Array.Reverse(buffer4);
            return BitConverter.ToInt32(buffer4, 0);
        }
        protected Int16 ReadInt16()
        {
            Stream.Read(buffer2, 0, 2);
            Array.Reverse(buffer2);
            return BitConverter.ToInt16(buffer2, 0);
        }
        protected UInt64 ReadUInt64()
        {
            Stream.Read(buffer8, 0, 8);
            Array.Reverse(buffer8);
            return BitConverter.ToUInt64(buffer8, 0);
        }
        protected Int64 ReadInt64()
        {
            Stream.Read(buffer8, 0, 8);
            Array.Reverse(buffer8);
            return BitConverter.ToInt64(buffer8, 0);
        }
        protected string ReadString4()
        {
            Stream.Read(buffer4, 0, 4);
            return Encoding.ASCII.GetString(buffer4);
        }
        protected string ReadString(int size)
        {
            var buffer = new byte[size];
            Stream.Read(buffer, 0, size);
            return Encoding.ASCII.GetString(buffer);
        }

        public override string ToString() => $"{Type} @{StartIndex}:{Size}:{UserType}";
        public static Box CreateBox(Stream stream, long startIndex)
        {
            var position = stream.Position;
            try
            {
                stream.Position = startIndex + 4;
                stream.Read(buffer4, 0, 4);
                var type = Encoding.ASCII.GetString(buffer4);
                switch (type)
                {
                    case "edts":
                    case "udta":
                    case "dinf": return new BoxNested(stream, startIndex);
                    case "moov": return new BoxMOOV(stream, startIndex);
                    case "trak": return new BoxTRAK(stream, startIndex);
                    case "mdia": return new BoxMDIA(stream, startIndex);
                    case "minf": return new BoxMINF(stream, startIndex);
                    case "stbl": return new BoxSTBL(stream, startIndex);
                    case "hdlr": return new BoxHDLR(stream, startIndex);
                    case "ftyp": return new BoxFTYP(stream, startIndex);
                    case "tkhd": return new BoxTKHD(stream, startIndex);
                    case "stts": return new BoxSTTS(stream, startIndex);
                    case "stsz": return new BoxSTSZ(stream, startIndex);
                    case "stco": return new BoxSTCO(stream, startIndex);
                    case "stss": return new BoxSTSS(stream, startIndex);
                    case "stsc": return new BoxSTSC(stream, startIndex);
                    case "stsd": return new BoxSTSD(stream, startIndex);
                    case "elst": return new BoxELST(stream, startIndex);
                    case "HMMT": return new BoxHMMT(stream, startIndex);
                    default: return new Box(stream, startIndex);
                }
            }
            finally
            {
                stream.Position = position;
            }
        }
    }
}
