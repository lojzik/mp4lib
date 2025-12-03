using Jha.Common;
using System;
using System.IO;

namespace Jha.MP4;

public class BoxSTTS(SubStream stream) : CountedListBox<BoxSTTS.Sample>(stream)
{
    public record Sample
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
