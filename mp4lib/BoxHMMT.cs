
using System;
using System.IO;

namespace JHa.MP4
{
    public class BoxHMMT : Box
    {
        public BoxHMMT(Stream stream, long startIndex) : base(stream, startIndex)
        {
        }
        public UInt32 EntryCount { get; private set; }
        public UInt32[] HighlightsTable { get; private set; }
        protected override void ReadData()
        {
            base.ReadData();
            EntryCount = ReadUInt32();
            HighlightsTable = new UInt32[EntryCount];
            for (int i = 0; i < EntryCount; i++)
            {
                HighlightsTable[i] = ReadUInt32();
            }
        }
    }
}
