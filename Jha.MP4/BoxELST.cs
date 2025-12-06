
using Jha.Common;
using System;
using System.IO;

namespace Jha.MP4;

[Box("elst")]
public class BoxELST(SubStream stream) : CountedListBox<BoxELST.Sample>(stream)
{
    public struct Sample
    {
        public UInt64 SegmentDuration { get; set; }
        public Int64 MediaTime { get; set; }
        public UInt32 SampleDescriptionIndex { get; set; }
        public Int16 MediaRateInteger { get; set; }
        public Int16 MediaRateFraction { get; set; }
    }

    protected override Sample ReadItem()
    {
        return new Sample()
        {
            SegmentDuration = Version == 1 ? ReadUInt64() : ReadUInt32(),
            MediaTime = Version == 1 ? ReadInt64() : ReadInt32(),
            MediaRateInteger= ReadInt16(),
            MediaRateFraction= ReadInt16(),
        };
    }
}
