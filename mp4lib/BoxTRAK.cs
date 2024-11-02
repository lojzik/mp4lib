
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace JHa.MP4
{
    public class BoxTRAK : BoxNested
    {
        public BoxTRAK(SubStream stream) : base(stream)
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
        public void ReadSample(uint index, Span<byte> buffer) => MDIA.MINF.STBL.ReadSample(index, buffer);
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
            var sampleCount = SampleCount;
            for (uint i = 0; i < sampleCount; i++)
                WriteTo(i, stream);
        }

    }
}
