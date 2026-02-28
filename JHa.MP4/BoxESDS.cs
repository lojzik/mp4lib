
using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("esds")]
public class BoxESDS(SubStream stream) : FullBox<UnknownFlags>(stream)
{
    public byte[] ESDescriptorData { get; private set; } = [];

    protected override void ReadData()
    {
        base.ReadData();
        int remaining = (int)(Size - Stream.Position);
        if (remaining > 0)
        {
            ESDescriptorData = new byte[remaining];
            Stream.ReadExactly(ESDescriptorData, 0, remaining);
        }
    }

    public override string ToString() => $"{base.ToString()}: ESDescriptorLength={ESDescriptorData.Length}";
}
