
using System;
using System.IO;

namespace JHa.MP4
{
    public class BoxTKHD : FullBox
    {
        public UInt64 CreationTime { get; private set; }
        public UInt64 ModificationTime { get; private set; }
        public UInt32 TrackId { get; private set; }
        public UInt64 Duration { get; private set; }
        public BoxTKHD(SubStream stream) : base(stream)
        {
        }
        protected override void ReadData()
        {
            base.ReadData();
            if (Version == 1)
            {
                CreationTime = ReadUInt64();
                ModificationTime = ReadUInt64();
                TrackId = ReadUInt32();
                ReadUInt32(); //reserved
                Duration = ReadUInt64();
            }
            else
            {
                CreationTime = ReadUInt32();
                ModificationTime = ReadUInt32();
                TrackId = ReadUInt32();
                ReadUInt32();//reserved
                Duration = ReadUInt32();
            }
            //todo: read next params
        }
        public override string ToString() => $"{base.ToString()}: {TrackId}";
    }
}
