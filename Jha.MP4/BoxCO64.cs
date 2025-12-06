
using Jha.Common;
using System;
using System.IO;

namespace Jha.MP4;

[Box("co64")]
public class BoxCO64(SubStream stream) : CountedListBox<UInt64>(stream)
{
    protected override ulong ReadItem() => ReadUInt64();
}
