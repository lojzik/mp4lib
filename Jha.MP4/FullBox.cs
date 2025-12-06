using Jha.Common;
using System;
using System.IO;

namespace Jha.MP4;

public enum UnknownFlags : uint
{
    None = 0
}
public class FullBox<T>(SubStream stream) : Box(stream) where T : Enum
{
    public byte Version { get; private set; }
    public T Flags { get; private set; } = default!;
    protected override void ReadData()
    {
        base.ReadData();
        Version = (byte)Stream.ReadByte();
//        Stream.ReadExactly(Flags, 0, 3);
        byte flag1 = (byte)Stream.ReadByte();
        byte flag2 = (byte)Stream.ReadByte();
        byte flag3 = (byte)Stream.ReadByte();
        var flags = (UInt32)((flag1 << 16) | (flag2 << 8) | flag3);
        Flags = (T)(object)flags;
    }
    public override string ToString() => $"{base.ToString()}: Version={Version}, Flags={Flags}";
}
