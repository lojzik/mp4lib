
using System;
using System.IO;

namespace JHa.MP4
{
    public class BoxSTTS : CountedListBox<BoxSTTS.Sample> //FullBox
    {
        public class Sample
        {
            public UInt32 Count { get; set; }
            public UInt32 Delta { get; set; }
        }
        public BoxSTTS(SubStream stream) : base(stream)
        {
        }

        protected override Sample ReadItem()
        {
            return new Sample()
            {
                Count = ReadUInt32(),
                Delta = ReadUInt32()
            };
        }
    }
}
