using Jha.Common;
using System;
using System.IO;

namespace Jha.MP4;

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
