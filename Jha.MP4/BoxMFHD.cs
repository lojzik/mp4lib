using JHa.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHa.MP4;

[Box("mfhd")]
public class BoxMFHD(SubStream stream) : FullBox<UnknownFlags>(stream)
{
    public UInt32 SequenceNumber { get; private set; }
    protected override void ReadData()
    {
        base.ReadData();
        SequenceNumber = ReadUInt32();
    }
    public override string ToString() => $"{base.ToString()}: SequenceNumber={SequenceNumber}";
}
