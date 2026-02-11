
using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("stss")]
public class BoxSTSS(SubStream stream) : CountedListBox<UInt32>(stream)
{
    protected override uint ReadItem() => ReadUInt32();
}
