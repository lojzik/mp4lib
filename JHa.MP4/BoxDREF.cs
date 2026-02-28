
using JHa.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace JHa.MP4;

[Box("dref")]
public class BoxDREF(SubStream stream) : FullBox<UnknownFlags>(stream)
{
    public UInt32 EntryCount { get; private set; }
    public List<Box> Entries { get; } = [];

    protected override void ReadData()
    {
        base.ReadData();
        EntryCount = ReadUInt32();
        for (uint i = 0; i < EntryCount; i++)
        {
            var entry = BoxFactory.Instance.CreateBox(Stream, Stream.Position);
            Entries.Add(entry);
        }
    }

    public override string ToString() => $"{base.ToString()}: EntryCount={EntryCount}";
}
