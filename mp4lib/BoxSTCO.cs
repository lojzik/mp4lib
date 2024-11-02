
using System;
using System.IO;

namespace JHa.MP4
{
    public class BoxSTCO : CountedListBox<UInt32>
    {
        public BoxSTCO(SubStream stream) : base(stream)
        {
        }
        protected override uint ReadItem() => ReadUInt32();
    }
}
