
using System;
using System.IO;

namespace Jha.MP4;

public class BoxSTCO(SubStream stream) : CountedListBox<UInt32>(stream)
{
    protected override uint ReadItem() => ReadUInt32();
}
