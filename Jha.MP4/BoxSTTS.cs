using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("stts")]
public class BoxSTTS(SubStream stream) : CountedListBox<BoxSTTS.Sample>(stream)
{
    public struct Sample
    {
        public UInt32 Count { get; set; }
        public UInt32 Delta { get; set; }
    }

    protected override BoxSTTS.Sample ReadItem()
    {
        return new BoxSTTS.Sample()
        {
            Count = ReadUInt32(),
            Delta = ReadUInt32()
        };
    }
}
