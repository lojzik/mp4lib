
using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("stsc")]
public class BoxSTSC(SubStream stream) : CountedListBox<BoxSTSC.Sample>(stream)
{
    public struct Sample
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
