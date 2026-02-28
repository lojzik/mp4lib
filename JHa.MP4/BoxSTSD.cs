using JHa.Common;
using System;
using System.IO;

namespace JHa.MP4;

[Box("stsd")]
public class BoxSTSD(SubStream stream) : FullBox<UnknownFlags>(stream)
{
    public UInt32 EntryCount { get; private set; }
    public Box[] Items { get; private set; } = [];

    protected override void ReadData()
    {
        base.ReadData();
        EntryCount = ReadUInt32();
        Items = new Box[EntryCount];
        for (int i = 0; i < EntryCount; i++)
        {
            Items[i] = BoxFactory.Instance.CreateBox(Stream, Stream.Position);
        }
    }

    public override string ToString() => $"{base.ToString()}: items:{EntryCount}";
}
