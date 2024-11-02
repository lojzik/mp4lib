
using System;
using System.IO;

namespace JHa.MP4
{
    public class BoxCO64 : CountedListBox<UInt64>
    {
        public BoxCO64(SubStream stream) : base(stream)
        {
        }
        protected override ulong ReadItem() => ReadUInt64();
    }
}
