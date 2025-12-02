
using System;
using System.IO;

namespace Jha.MP4;

public class BoxCO64(SubStream stream) : CountedListBox<UInt64>(stream)
{
    protected override ulong ReadItem() => ReadUInt64();
}
