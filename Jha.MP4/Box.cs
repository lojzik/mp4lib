
using Jha.Common;
using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Jha.MP4;

public class Box 
{
    protected SubStream Stream { get; }
    public long Size => Stream.Length;
    public long StartPosition => Stream.StartPosition;
    public long? ContentPosition;
    public long ContentSize => Size - (ContentPosition ?? 0);
    public String4 Type;
    public Guid? UserType { get; private set; }
    public Box(SubStream stream)
    {
        Stream = stream;
        ReadData();
    }
    public void ReadContentData(byte[] buffer)
    {
        Stream.Position = (long)ContentPosition!.Value;
        Stream.ReadExactly(buffer, 0, (int)(ContentSize));
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
        if (Type.Equals(BoxName.uuid))
        {
            Span<byte> buffer = stackalloc byte[16];
            Stream.ReadExactly(buffer);
            UserType = new Guid(buffer);
        }
        ContentPosition = Stream.Position;
    }
    protected delegate T Converter<T>(ReadOnlySpan<byte> args);
    protected T Read<T>(Converter<T> converter) where T : struct
    {
        var size = Unsafe.SizeOf<T>();
        Span<byte> buff = stackalloc byte[size];
        Stream.ReadExactly(buff);
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
        Stream.ReadExactly(buff);
        return Encoding.ASCII.GetString(buff);
    }
    protected void Read(ref String4 str)
    {
        unsafe
        {
            fixed (byte* p = str.ByteBuffer)
            {
                var span = new Span<byte>(p, 4);
                Stream.ReadExactly(span);
            }
        }
    }
    public override string ToString() => $"{Type}<{GetType().Name}> @{Stream.StartPosition}:{Stream.Length}({(Stream.StartPosition + Stream.Length - 1)}):{UserType}";
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
                stream.ReadExactly(x);
            }
            var substream = new SubStream(stream, startIndex);
            return xtype.AsString switch
            {
                "edts" or "udta" or "dinf" => new BoxNested(substream),
                "moov" => new BoxMOOV(substream),
                "trak" => new BoxTRAK(substream),
                "mdia" => new BoxMDIA(substream),
                "minf" => new BoxMINF(substream),
                "stbl" => new BoxSTBL(substream),
                "hdlr" => new BoxHDLR(substream),
                "ftyp" => new BoxFTYP(substream),
                "tkhd" => new BoxTKHD(substream),
                "stts" => new BoxSTTS(substream),
                "stsz" => new BoxSTSZ(substream),
                "stco" => new BoxSTCO(substream),
                "stss" => new BoxSTSS(substream),
                "stsc" => new BoxSTSC(substream),
                "stsd" => new BoxSTSD(substream),
                "elst" => new BoxELST(substream),
                "HMMT" => new BoxHMMT(substream),
                "co64" => new BoxCO64(substream),
                _ => new Box(substream),
            };
        }
        finally
        {
            stream.Position = position;
        }
    }
}
