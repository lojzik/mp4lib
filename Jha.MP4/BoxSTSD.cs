using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("stsd")]
public class BoxSTSD(SubStream stream) : CountedListBox<BoxSTSD.SampleEntry>(stream)
{
    public class SampleEntry(SubStream stream) : Box(stream)
    {
    }

    protected override SampleEntry ReadItem()
    {
        return new SampleEntry(Stream)
        {
        };
    }
}
