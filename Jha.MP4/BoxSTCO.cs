
using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("stco")]
public class BoxSTCO(SubStream stream) : CountedListBox<UInt32>(stream)
{
    protected override uint ReadItem() => ReadUInt32();
}
