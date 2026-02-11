using JHa.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHa.MP4;

[Box("tfhd")]
public class BoxTFHD(SubStream stream) : FullBox<BoxTFHD.TfhdFlags>(stream)
{
    [Flags]
    public enum TfhdFlags : uint
    {
        None = 0x000000,
        BaseDataOffsetPresent = 0x000001,
        SampleDescriptionIndexPresent = 0x000002,
        DefaultSampleDurationPresent = 0x000008,
        DefaultSampleSizePresent = 0x000010,
        DefaultSampleFlagsPresent = 0x000020,
        DurationIsEmpty = 0x010000,
        DefaultBaseIsMoof = 0x020000
    }
    public UInt32 TrackId { get; private set; }
    protected override void ReadData()
    {
        base.ReadData();
        TrackId = ReadUInt32();
    }
    public override string ToString() => $"{base.ToString()}: TrackIdr={TrackId}";
}
