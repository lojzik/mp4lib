
using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("smhd")]
public class BoxSMHD(SubStream stream) : FullBox<UnknownFlags>(stream)
{
    public Int16 Balance { get; private set; }

    protected override void ReadData()
    {
        base.ReadData();
        Balance = ReadInt16();
        ReadInt16(); // reserved
    }

    public override string ToString() => $"{base.ToString()}: Balance={Balance}";
}
