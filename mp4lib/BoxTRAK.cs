
using System;
using System.IO;

namespace JHa.MP4
{
    public class BoxTRAK : BoxNested
    {
        public BoxTRAK(Stream stream, long startIndex) : base(stream, startIndex)
        {
        }
        public BoxMDIA MDIA => FindBox<BoxMDIA>("mdia");
        public BoxTKHD TKHD => FindBox<BoxTKHD>("tkhd");
        public string StreamName => MDIA.HDLR.ComponentName;
        public UInt32 TrackId => TKHD.TrackId;
        public override string ToString() => $"{base.ToString()}: {TrackId} {StreamName}";
        public UInt32 SampleCount => MDIA.MINF.STBL.STSZ.SampleCount;
        public UInt32 SampleSize(uint index) => MDIA.MINF.STBL.STSZ.SampleSize(index);
        public void ReadSample(uint index, byte[] buffer, int offset) => MDIA.MINF.STBL.ReadSample(index, buffer, offset);
        public UInt32 TotalSize => MDIA.MINF.STBL.STSZ.TotalSize;
        public void WriteTo(uint index, Stream stream)
        {
            var sampleSize = SampleSize(index);
            var buffer = new byte[sampleSize];
            ReadSample(index, buffer, 0);
            stream.Write(buffer, 0, (int)sampleSize);
        }
        public void WriteAllTo(Stream stream)
        {
            for (uint i = 0; i < SampleCount; i++)
                WriteTo(i, stream);
        }

    }
}
