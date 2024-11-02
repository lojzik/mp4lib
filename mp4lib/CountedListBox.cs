
using System;
using System.IO;

namespace JHa.MP4
{
    public abstract class CountedListBox<T> : FullBox
    {
        public CountedListBox(SubStream stream) : base(stream)
        {
        }
        public UInt32 EntryCount { get; protected set; }
        public T[] Items { get; protected set; } = Array.Empty<T>();
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
}
