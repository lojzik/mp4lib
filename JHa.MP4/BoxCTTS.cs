
using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("ctts")]
public class BoxCTTS(SubStream stream) : CountedListBox<BoxCTTS.Sample>(stream)
{
    public struct Sample
    {
        public UInt32 SampleCount { get; set; }
        public Int64 SampleOffset { get; set; }
    }

    protected override Sample ReadItem()
    {
        return new Sample()
        {
            SampleCount = ReadUInt32(),
            SampleOffset = Version == 0 ? ReadUInt32() : ReadInt32()
        };
    }
}
