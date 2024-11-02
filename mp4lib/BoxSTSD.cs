
using System;
using System.IO;

namespace JHa.MP4
{
    public class BoxSTSD : CountedListBox<BoxSTSD.SampleEntry>
    {
        public class SampleEntry : Box
        {
            public SampleEntry(SubStream stream) : base(stream)
            {
            }
        }
        public BoxSTSD(SubStream stream) : base(stream)
        {
        }
        protected override SampleEntry ReadItem()
        {
            return new SampleEntry(Stream)
            {
            };
        }
    }
}
