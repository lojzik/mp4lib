using Jha.Common;
using System;
using System.IO;

namespace Jha.MP4;

public abstract class CountedListBox<T>(SubStream stream) : FullBox(stream)
{
    public UInt32 EntryCount { get; protected set; }
    public T[] Items { get; protected set; } = [];
    protected override void ReadData()
    {
        base.ReadData();
        EntryCount = ReadUInt32();
        Items = new T[EntryCount];
        for (int i = 0; i < EntryCount; i++)
        {
            Items[i] = ReadItem();
        }
    }
    protected abstract T ReadItem();
    public override string ToString() => $"{base.ToString()}: items:{EntryCount}";
}
