
using System;
using System.IO;

namespace JHa.MP4
{
    public class BoxSTSC : CountedListBox<BoxSTSC.Sample>
    {
        public class Sample
        {
            public UInt32 FirstChunk { get; set; }
            public UInt32 SamplesPerChunk { get; set; }
            public UInt32 SampleDescriptionIndex { get; set; }
        }
        public BoxSTSC(SubStream stream) : base(stream)
        {
        }
        protected override Sample ReadItem()
        {
            return new Sample()
            {
                FirstChunk = ReadUInt32(),
                SamplesPerChunk = ReadUInt32(),
                SampleDescriptionIndex = ReadUInt32()
            };
        }
    }
}
