
using System;
using System.IO;

namespace Jha.MP4;

public class BoxSTSC(SubStream stream) : CountedListBox<BoxSTSC.Sample>(stream)
{
    public class Sample
    {
        public UInt32 FirstChunk { get; set; }
        public UInt32 SamplesPerChunk { get; set; }
        public UInt32 SampleDescriptionIndex { get; set; }
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
