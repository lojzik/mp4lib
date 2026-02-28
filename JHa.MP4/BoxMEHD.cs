
using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("mehd")]
public class BoxMEHD(SubStream stream) : FullBox<UnknownFlags>(stream)
{
    public UInt64 FragmentDuration { get; private set; }

    protected override void ReadData()
    {
        base.ReadData();
        FragmentDuration = Version == 1 ? ReadUInt64() : ReadUInt32();
    }

    public override string ToString() => $"{base.ToString()}: FragmentDuration={FragmentDuration}";
}
