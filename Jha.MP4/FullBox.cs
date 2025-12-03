using Jha.Common;
using System.IO;

namespace Jha.MP4;

public class FullBox(SubStream stream) : Box(stream)
{
    public byte Version { get; private set; }
    public byte[] Flags { get; } = new byte[3];
    protected override void ReadData()
    {
        base.ReadData();
        Version = (byte)Stream.ReadByte();
        Stream.ReadExactly(Flags, 0, 3);
    }
    public override string ToString() => $"{base.ToString()}: version: {Version}";
}
