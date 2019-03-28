
using System;
using System.IO;

namespace JHa.MP4
{
    public class BoxELST : CountedListBox<BoxELST.Sample>
    {
        public class Sample
        {
            public UInt64 SegmentDuration { get; set; }
            public Int64 MediaTime { get; set; }
            public UInt32 SampleDescriptionIndex { get; set; }
            public Int16 MediaRateInteger { get; set; }
            public Int16 MediaRateFraction { get; set; }
        }
        public BoxELST(Stream stream, long startIndex) : base(stream, startIndex)
        {
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
}
