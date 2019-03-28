
using System;
using System.IO;

namespace JHa.MP4
{
    public class BoxSTCO : CountedListBox<UInt32>
    {
        public BoxSTCO(Stream stream, long startIndex) : base(stream, startIndex)
        {
        }
        protected override uint ReadItem() => ReadUInt32();
    }
}
