
using System;
using System.IO;

namespace JHa.MP4
{
    public class BoxSTSS : CountedListBox<UInt32>
    {
        public BoxSTSS(SubStream stream) : base(stream)
        {
        }
        protected override uint ReadItem() => ReadUInt32();
    }
}
