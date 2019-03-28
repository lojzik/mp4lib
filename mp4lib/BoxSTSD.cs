
using System;
using System.IO;

namespace JHa.MP4
{
    public class BoxSTSD : CountedListBox<BoxSTSD.SampleEntry>
    {
        public class SampleEntry : Box
        {
            public SampleEntry(Stream stream, long startIndex) : base(stream, startIndex)
            {
            }
        }
        public BoxSTSD(Stream stream, long startIndex) : base(stream, startIndex)
        {
        }
        protected override SampleEntry ReadItem()
        {
            return new SampleEntry(Stream,Stream.Position)
            {
            };
        }
    }
}
