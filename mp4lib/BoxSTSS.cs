
using System;
using System.IO;

namespace JHa.MP4
{
    public class BoxSTSS : CountedListBox<UInt32>
    {
        public BoxSTSS(Stream stream, long startIndex) : base(stream, startIndex)
        {
        }
        protected override uint ReadItem() => ReadUInt32();
    }
}
