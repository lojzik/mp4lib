
using JHa.Common;
using System;
using System.Drawing;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Schema;

namespace JHa.MP4;

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
    protected T[] ReadArray<T>(Converter<T> converter, int count) where T : struct
    {
        T[] result = new T[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = Read(converter);
        }
        return result;
    }
    protected UInt32 ReadUInt32() => Read(BitConverter.ToUInt32);
    protected Int32 ReadInt32() => Read(BitConverter.ToInt32);
    protected Int16 ReadInt16() => Read(BitConverter.ToInt16);
    protected UInt64 ReadUInt64() => Read(BitConverter.ToUInt64);
    protected Int64 ReadInt64() => Read(BitConverter.ToInt64);
    protected UInt32[] ReadUInt32Array(int count) => ReadArray(BitConverter.ToUInt32, count);
    protected Int32[] ReadInt32Array(int count) => ReadArray(BitConverter.ToInt32, count);

    protected String4 ReadString4()
    {
        Span<byte> buffer = stackalloc byte[4];
        Stream.ReadExactly(buffer);
        String4 str = new String4();
        str.From(buffer);
        return str;
    }
    protected string ReadString(int size)
    {
        Span<byte> buff = stackalloc byte[size];
        Stream.ReadExactly(buff);
        return Encoding.ASCII.GetString(buff);
    }
    protected void Read(ref String4 str)
    {
        Span<byte> buffer = stackalloc byte[4];
        Stream.ReadExactly(buffer);
        str.From(buffer);
    }
    public override string ToString() => $"{Type}<{GetType().Name}> @{Stream.StartPosition}:{Stream.Length}({(Stream.StartPosition + Stream.Length - 1)}):{UserType}";

}
