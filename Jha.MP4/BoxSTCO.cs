
using Jha.Common;
using System;
using System.IO;

namespace Jha.MP4;

[Box("stco")]
public class BoxSTCO(SubStream stream) : CountedListBox<UInt32>(stream)
{
    protected override uint ReadItem() => ReadUInt32();
}
